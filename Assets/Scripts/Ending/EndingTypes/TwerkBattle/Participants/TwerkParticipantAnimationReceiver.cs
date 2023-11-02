using System;
using UnityEngine;

public class TwerkParticipantAnimationReceiver : MonoBehaviour
{
    private ParticipantTwerkBattleAnimator _animator;

    private void Awake()
    {
        _animator = transform.parent.GetComponent<ParticipantTwerkBattleAnimator>();
    }

    public void DanceEnded()
    {
        _animator.DanceEnded();
    }

    public void Died()
    {
        _animator.Died();
    }

    public void PunchEnd()
    {
        _animator.PunchEnd();
    }

    public void HitEnemy()
    {
        _animator.HitEnemy();
    }

    public void StandUp()
    {
        _animator.StandUp();
    }
}
