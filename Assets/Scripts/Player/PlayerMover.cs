using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _runningBeamSpeed = 300f;
    private float _startingSpeed;

    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpTime;
    [SerializeField] private PlayerAnimator _playerAnimator;

    [Header("Timings")] [SerializeField] private float _stunSeconds;
    [SerializeField] private float _standUpCooldown;
    
    [Space(10)][Header("Jump Pad")]
    [SerializeField] private float _jumpPadHeight;
    [SerializeField] private float _horizontalForceMultiplier;
    [SerializeField] private float _verticalForceMultiplier;
    
    
    private PlayerInput _playerInput;
    private PlayerBeamMovement _playerBeamMovement;
    
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;

    private bool _isStunned;
    private bool _isLanded = true;
    private bool _isJumping;
    public bool IsJumping => _isJumping;
    public bool IsLanded => _isLanded;

    public bool OnTreadmill = false;

    private bool _insideBeam;

    public float YVelocityOffset = 0;
    public float HorizontalSpeed => _horizontalSpeed;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerBeamMovement = GetComponent<PlayerBeamMovement>();

        _startingSpeed = _runningSpeed;
    }
    
    private void Start()
    {
        ServiceLocator.Player.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        ServiceLocator.Player.Died -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        enabled = false;
    }


    public void TryMove()
    {
        if ((Input.GetMouseButton(0) || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") > 0)&& !_isStunned && !_isJumping)
        {
            Move();
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }

        if ((_isJumping || _insideBeam) && !_isStunned)
        {
            Move();
        }
    }

    private void Update()
    {
        if (OnTreadmill)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        }
    }

    private void Move()
    {
        var moveDirection = GetMoveDirection();
        _playerAnimator.TurningDirection = GetXDirection();
        _rigidbody.velocity = new Vector3(_insideBeam ? 0 : moveDirection.x, 0f + YVelocityOffset, moveDirection.z);
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 frontDirection = Vector3.forward;
        Vector3 horizontalDirection = new Vector3(_playerInput.GetXDirection(), 0f, 0f);
        frontDirection = frontDirection * _runningSpeed * Time.fixedDeltaTime;
        horizontalDirection = horizontalDirection * _horizontalSpeed * Time.fixedDeltaTime;
        return horizontalDirection + frontDirection;
    }

    public float GetXDirection()
    {
        return _playerInput.GetXDirection();
    }

    public void Push(float pushPower)
    {
        transform.DOMoveZ(transform.position.z - pushPower, 0.5f);
        _playerAnimator.AnimateObstacleFall();
        Block(true);
    }

    public void Block(bool value)
    {
        _isStunned = value;
    }

     public Tween MoveTo(Vector3 target, float moveDuration)
     {
         _playerAnimator.SetRunningTrigger();
        _playerAnimator.SetPlayerAnimationState(PlayerAnimationState.Other);
        return _rigidbody.DOMove(target, moveDuration).SetEase(Ease.Linear);
                          //.OnComplete(() => _playerAnimator.SetPlayerAnimationState(PlayerAnimationState.Idle));
     }
     
     public Tween RestrictedJump(Transform target, float jumpPower, float duration)
    {
        Block(true);
        return _rigidbody.DOJump(target.position, jumpPower, 1, duration);
    }

    public void Land()
    {
        _isJumping = false;
        _isLanded = true;
        Block(false);
        _playerAnimator.SetRunningTrigger();
    }
    
    public void ChangeRunningSpeedBy(float value)
    {
        _runningSpeed += value;
    }

    public void EnterBeam()
    {
        _insideBeam = true;
        _runningSpeed = _runningBeamSpeed;
        _playerBeamMovement.StartBeamMovement();
    }

    public void ExitBeam()
    {
        _runningSpeed = _startingSpeed;
        _insideBeam = false;
        _playerBeamMovement.EndBeamMovement();
        transform.localRotation = Quaternion.identity;
    }

    public void BalanceOut()
    {
        _playerBeamMovement.BalanceItOut();
    }

}
