using System;
using DG.Tweening;
using UnityEngine;

public class PunchMovingEnemy : MonoBehaviour, IObstacle
{
    [SerializeField] private float _pushPower;
    [SerializeField] private Transform _girlModel;
    [SerializeField] private Vector3[] _movingPoints;
    [SerializeField] private float _movingDuration;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _damage;
    [SerializeField] private AudioClip _hitSound;
    
    private AnimationState _state;
    private static readonly int StrafeLeft = Animator.StringToHash("StrafeLeft");
    private static readonly int StrafeRight = Animator.StringToHash("StrafeRight");
    private static readonly int Push = Animator.StringToHash("Punch");

    public void AffectPlayer(Player player)
    {
        player.Push(_pushPower);
        player.TryApplyDamage(_damage);
        _animator.SetTrigger(Push);
        AnimateGirl();
        SoundManager.Instance.PlaySound(_hitSound);
    }
    
    private void Start()
    {
        InitializePoints();
        InitializeMove();
    }

    private void InitializePoints()
    {
        for (int i = 0; i < _movingPoints.Length; i++)
        {
            _movingPoints[i] += transform.position;
        }
    }

    private void InitializeMove()
    {
        Sequence infiniteSequence = DOTween.Sequence().SetLoops(-1);
        infiniteSequence.Append(_girlModel.DOMove(_movingPoints[1], _movingDuration).From(_movingPoints[0]).OnStart(
            () =>
            {
                _animator.SetTrigger(StrafeLeft);
                _state = AnimationState.MovingLeft;
            }).SetEase(Ease.Linear));
        infiniteSequence.Append(_girlModel.DOMove(_movingPoints[0], _movingDuration).From(_movingPoints[1]).OnStart(
            () =>
            {
                _animator.SetTrigger(StrafeRight);
                _state = AnimationState.MovingRight;
            }).SetEase(Ease.Linear));
    }

    private void AnimateGirl()
    {
        switch (_state)
        {
            case AnimationState.MovingLeft:
                _animator.SetTrigger(StrafeLeft);
                break;
            case AnimationState.MovingRight:
                _animator.SetTrigger(StrafeRight);
                break;
        }
    }
}

enum AnimationState
{
    MovingLeft,
    MovingRight
}
