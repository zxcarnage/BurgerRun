using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class EnemyTwerkBattle : Participant, IParticipant
{
    private Participant _playerParticipant;
    private float _damage;
    public override event Action TurnEnded;
    public override void Positionate()
    {
        var offset = Mathf.Lerp(0f, _offsetMax, _hp);
        transform.position = new Vector3(transform.position.x + offset, transform.position.y,transform.position.z);
    }

    protected override void Die()
    {
        enabled = false;
    }

    public void GiveEnemy(Participant participant)
    {
        _playerParticipant = participant;
        var hp = _playerStats.Health - _playerStats.Health / 10;
        _maxHP = hp;
        InitializeSizes(hp);
        _playerParticipant.TurnEnded += OnPlayerParticipantTurnEnded;
    }
    
    private void OnDisable()
    {
        if (_playerParticipant == null)
            return;
        _playerParticipant.TurnEnded -= OnPlayerParticipantTurnEnded;
    }

    private void OnPlayerParticipantTurnEnded()
    {
        if(TryDie())
            return;
        StartCoroutine(OnPlayerParticipantTurnEndedRoutine());
    }

    private IEnumerator OnPlayerParticipantTurnEndedRoutine()
    {
        TurnStarted();
        yield return new WaitUntil(() => CameraChanged);
        CalculateDamage();
        yield return new WaitUntil(() => _animator.StandedUp);
        _animator.PlayPunchAnimation();
        yield return new WaitUntil(() => _animator.PunchApplyied);
        DealDamage();
        yield return new WaitUntil(() => _animator.EnemyStandedUp || _playerParticipant.Animator.EnemyDied);
        Debug.Log("Invoked turn ended enemy");
        TurnEnded?.Invoke();
    }

    private void CalculateDamage()
    {
        int minDamage = Convert.ToInt16(_playerStats.MaxHealth / 6);
        int maxDamage = Convert.ToInt16(_playerStats.MaxHealth / 4);
        _damage = new Random().Next(minDamage, maxDamage);
    }

    private void DealDamage()
    {
        _playerParticipant.Animator.ChangeHitAnimation(_playerParticipant.Health, _playerParticipant.MaxHealth, _damage);
        _playerParticipant.TryApplyDamage(_damage);
    }

}
