using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(ParticipantTwerkBattleAnimator))]
public abstract class Participant : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private int _participantWaypoint;
    [SerializeField] private Healthbar _participantHealthbar;
    [SerializeField] protected PlayerRuntimeStats _playerStats;
    [SerializeField][Range(0,1f)] protected float _offsetMax;

    protected float _maxHP;
    protected float _hp;
    protected ParticipantTwerkBattleAnimator _animator;
    private CinemachineTrackedDolly _dolly;
    private bool _cameraChanged;
    protected bool CameraChanged => _cameraChanged;
    public float Health => _hp;
    public float MaxHealth => _maxHP;
    public ParticipantTwerkBattleAnimator Animator => _animator;

    public abstract event Action TurnEnded;
    public event Action Died;
    public event Action<Participant> HealthChanged;
    
    private void Awake()
    {
        _dolly = _camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        _animator = GetComponent<ParticipantTwerkBattleAnimator>();
    }

    protected void TurnStarted()
    {
        StartCoroutine(ChangeWaypoint());
    }

    public void TryApplyDamage(float damage)
    {
        _cameraChanged = false;
        if (damage < 0)
            return;
        ApplyDamage(damage);
    }
    

    private void ApplyDamage(float damage)
    {
        _hp -= damage;
        HealthChanged?.Invoke(this);
    }

    protected bool TryDie()
    {
        if (_hp <= 0)
        {
            Die();
            Died?.Invoke();
            return true;
        }

        return false;
    }

    protected void InitializeSizes(float health)
    {
        _hp = health;
        HealthChanged?.Invoke(this);
    }

    public void ChangeHealthbarValue()
    {
        _participantHealthbar.ChangeHealth(_hp, _maxHP);
    }

    public void InitializeHealthbars()
    {
        _participantHealthbar.Initialize(_hp, _maxHP);
    }

    public abstract void Positionate();

    protected abstract void Die();

    private IEnumerator ChangeWaypoint()
    {
        _dolly.m_PathPosition = _participantWaypoint;
        yield return new WaitForSeconds(1);
        _cameraChanged = true;
    }


}
