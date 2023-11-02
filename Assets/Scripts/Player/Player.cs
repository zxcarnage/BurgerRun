using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerJumpMover))]
[RequireComponent(typeof(PlayerSkinSwitcher))]
[RequireComponent(typeof(PlayerCollisionHandler))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerRuntimeStats _stats;
    [SerializeField] private AudioClip _jumpSound;

    private float _currentHealth;
    private PlayerMover _playerMover;
    private PlayerJumpMover _playerJump;
    private PlayerCollisionHandler _collisionHandler;
    private PlayerSkinSwitcher _skinSwitcher;
    private bool _isJumping;
    private bool _movementBlocked;
    
    public event Action<float,float> HealthChanged;
    public event Action Died;
    public float Health => _currentHealth;
    public float MaxHealth => _maxHealth;
    public PlayerAnimator PlayerAnimator => _playerAnimator;
    public PlayerMover PlayerMover => _playerMover;

    public PlayerRollingMovement PlayerRollingMovement { get; private set; }

    private void Awake()
    {
        ServiceLocator.Player = this;
        _playerMover = GetComponent<PlayerMover>();
        _playerJump = GetComponent<PlayerJumpMover>();
        _skinSwitcher = GetComponent<PlayerSkinSwitcher>();
        _collisionHandler = GetComponent<PlayerCollisionHandler>();
        PlayerRollingMovement = GetComponent<PlayerRollingMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IEatable food))
            _collisionHandler.HandleFoodCollision(this, food);
        else if (other.gameObject.TryGetComponent(out ILine line))
            _collisionHandler.HandleLineTrigger(this, line);
        else if (other.gameObject.TryGetComponent(out IObstacle obstacle))
            _collisionHandler.HandleObstacleCollision(this, obstacle);
        else if (other.gameObject.TryGetComponent(out IEnemy enemy))
            _collisionHandler.HandleEnemyTrigger(this, enemy);
        else if (other.TryGetComponent(out IZone zone))
            _collisionHandler.HandleZoneTrigger(this, zone);
        else if (other.TryGetComponent(out IAreaTrigger area))
            _collisionHandler.HandleAreaTriggerEnter(this, area);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isJumping && collision.gameObject.tag == "Floor")
        {
            _playerAnimator.JumpEnded();
            UnlockMovement();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IAreaTrigger area))
        {
            _collisionHandler.HandleAreaTriggerStay(this, area);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IAreaTrigger area))
        {
            _collisionHandler.HandleAreaTriggerExit(this, area);
        }
    }
    

    public void Push(float pushPower)
    {
        _playerMover.Push(pushPower);
    }

    public void Fall(float value)
    {
        _playerAnimator.AnimateBeamFall(value);
    }

    public void AnimatePunch(Direction direction)
    {
        _playerAnimator.AnimatePunch(direction);
    }

    public void BlockMovement()
    {
        _movementBlocked = true;
    }

    public void SwitchSkin(Skin newSkin)
    {
        _skinSwitcher.Switch(newSkin);
    }

    public void UnlockMovement()
    {
        _movementBlocked = false;
        if (_isJumping) _isJumping = false;
    }
    
    public void TryApplyDamage(float damage)
    {
        if (damage > 0)
        {
            if (_currentHealth >= damage)
                ApplyDamage(damage);
            else
                ResetHealth(0);
        }
        else
        {
            damage *= -1;
            var difference = _maxHealth - _currentHealth;
            if(difference >= damage)
                ApplyDamage(-damage);
            else
                ResetHealth(_maxHealth);
        }
    }

    public void Die()
    {
        ServiceLocator.GetEndCanvas().ShowLosePanel();
    }

    private void ResetHealth(float value)
    {
        _currentHealth = value;
        _stats.SetHealth(value);
        HealthChanged?.Invoke(_currentHealth,_maxHealth);
    }

    private void ApplyDamage(float damage)
    {
        _currentHealth -= damage;
        _stats.ReduceHealth(damage);
        HealthChanged?.Invoke(_currentHealth,_maxHealth);
    }

    private void FixedUpdate()
    {
        if(!_movementBlocked)
            _playerMover.TryMove();

        if (Input.GetKeyDown(KeyCode.H))
            Debug.Log("Current health = " + _currentHealth);
    }

    public Tween DoMove(Vector3 target,float moveDuration)
    {
       return _playerMover.MoveTo(target,moveDuration);
    }

    public void JumpTo(Transform jumpTarget)
    {
        _playerAnimator.StartJumpAnimation();
        SoundManager.Instance.PlaySound(_jumpSound);
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        _playerMover.RestrictedJump(jumpTarget, 3, 1f).OnComplete(() => _playerAnimator.JumpEnded()).SetEase(Ease.Linear);
    }

    public void PadJumpTo(Transform destination, bool isOnPad)
    {
        SoundManager.Instance.PlaySound(_jumpSound);
        _isJumping = true;
        _playerJump.JumpToTarget(destination, isOnPad);
    }

    public void GiveAdditionalPower(float value)
    {
        if (value > 0)
        {
            _currentHealth += value;
            _stats.AddHealth(value);
        }
            
    }

    public void FallInToPit()
    {
        BlockMovement();
        _playerMover.Rigidbody.velocity = Vector3.zero;
        _playerAnimator.AnimatePitFall();
        transform.DORotate(new Vector3(90, 0, 0), 1f);
        Camera.Instance.StopFollowing();
        Died?.Invoke();
        FallingDie(1.5f);
        Invoke("Die", 1.5f);
    }

    public void DisableCollider()
    {
        transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = false;
    }

    public void FallInToPool()
    {
        BlockMovement();
        Camera.Instance.StopFollowing();
        _playerMover.Rigidbody.velocity = Vector3.zero;
        Died?.Invoke();
        FallingDie(2f);
        Invoke("Die", 2f);
        Debug.Log("died");
    }

    private void FallingDie(float duration)
    {
        transform.DOMoveY(transform.position.y - 10f, duration);
    }
}
