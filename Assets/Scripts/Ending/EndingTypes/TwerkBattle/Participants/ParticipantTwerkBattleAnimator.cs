using System;
using UnityEngine;

public abstract class ParticipantTwerkBattleAnimator : MonoBehaviour
{
    [SerializeField] private ParticipantTwerkBattleAnimator _enemyAnimator;
    [SerializeField] protected Animator _animator;

    private bool _enemyDied = false;
    private bool _standedUp = false;
    private bool _punchEnded = true;
    private bool _punchApplyied;
    private bool _sexyDanceEnded;
    
    public bool StandedUp => _standedUp;
    public bool SexyDanceEnded => _sexyDanceEnded;
    public bool PunchApplyied => _punchApplyied;
    public bool PunchEnded => _punchEnded;
    public bool EnemyDied => _enemyDied;
    public bool EnemyStandedUp => _enemyAnimator.StandedUp;
    private static readonly int Twerk = Animator.StringToHash("Twerk");
    private static readonly int HitNum = Animator.StringToHash("Hit_Num");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Win = Animator.StringToHash("Win");
    private static readonly int Punch = Animator.StringToHash("Punch");

    private void Start()
    {
        _animator.SetTrigger(Twerk);
    }
    
    public void ChangeHitAnimation(float currentHP, float maxHP, float futureDamage)
    {
        var actualHP = currentHP - futureDamage;
        var hitNum = 0;
        if (actualHP > 0 && actualHP < 0.7f * maxHP)
            hitNum = 1;
        else if (actualHP <= 0)
            hitNum = 2;
        _animator.SetInteger(HitNum,hitNum);
    }

    public void PlayPunchAnimation()
    {
        _punchEnded = false;
        _animator.SetTrigger(Punch);
    }

    public void DanceEnded()
    {
        _sexyDanceEnded = true;
    }

    public void PunchEnd()
    {
        _punchEnded = true;
        _punchApplyied = false;
    }

    public void Died()
    {
        _enemyDied = true;
    }

    public void HitEnemy()
    {
        _punchApplyied = true;
        _enemyAnimator.PlayHitAnimation();
    }

    public void StandUp()
    {
        _standedUp = true;
    }

    public void PlayHitAnimation()
    {
        _animator.SetTrigger(Hit);
        _standedUp = false;
    }

    public void WinAnimation()
    {
        _animator.SetTrigger(Win);
    }
    
}
