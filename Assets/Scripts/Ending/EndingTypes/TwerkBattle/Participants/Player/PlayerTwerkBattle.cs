using System;
using System.Collections;
using UnityEngine;

public class PlayerTwerkBattle : Participant, IParticipant
{
    [SerializeField] private Roulette _twerkRoulette;
    [SerializeField] private TwerkPlayerSkinSwitcher _switcher;
    private Participant _enemy;

    public override event Action TurnEnded;
    public override void Positionate()
    {
        var offset = Mathf.Lerp(0f, _offsetMax, _hp);
        transform.position = new Vector3(transform.position.x - offset, transform.position.y,transform.position.z);
    }

    protected override void Die()
    {
        ServiceLocator.GetEndCanvas().ShowLosePanel();
        Destroy(gameObject);
    }

    public void Initialize()
    {
        _switcher.Initialize();
    }

    public void GiveEnemy(Participant participant)
    {
        _enemy = participant;
        var hp = _playerStats.Health;
        _maxHP = hp;
        InitializeSizes(hp);
        _enemy.TurnEnded += OnEnemyParticipantTurnEnded;
        _enemy.Died += OnEnemyDied;
        _twerkRoulette.SegmentChosen += OnSegmentChosen;
        FirstTurn();
    }

    private void FirstTurn()
    {
        Debug.Log("FirstTurn");
        _twerkRoulette.gameObject.SetActive(true);
    }
    

    private void OnDisable()
    {
        if (_enemy == null)
            return;
        _enemy.TurnEnded -= OnEnemyParticipantTurnEnded;
        _enemy.Died -= OnEnemyDied;
        _twerkRoulette.SegmentChosen -= OnSegmentChosen;
    }

    private void OnEnemyDied()
    {
        _twerkRoulette.transform.parent.gameObject.SetActive(false);
        StartCoroutine(WinRountine());
    }

    private IEnumerator WinRountine()
    {
        _animator.WinAnimation();
        yield return new WaitUntil(() => _animator.SexyDanceEnded);
        ServiceLocator.GetEndCanvas().ShowWinPanel(_hp / 10);
    }

    private void OnEnemyParticipantTurnEnded()
    {
        Debug.Log("On enemy participant turn ended");
        TryDie();
        StartCoroutine(OnPlayerParticipantTurnEndedRoutine());
    }

    private IEnumerator OnPlayerParticipantTurnEndedRoutine()
    {
        TurnStarted();
        yield return new WaitUntil(() => CameraChanged);
        Debug.Log("Setting roulette on");
        _twerkRoulette.gameObject.SetActive(true);
    }

    private void OnSegmentChosen(RouletteSegment segment)
    {
        _twerkRoulette.gameObject.SetActive(false);
        Debug.Log(_twerkRoulette.gameObject.activeSelf);
        StartCoroutine(DealDamageRountine(segment));
    }

    private IEnumerator DealDamageRountine(RouletteSegment segment)
    {
        _enemy.Animator.ChangeHitAnimation(_enemy.Health,_enemy.MaxHealth, segment.Value);
        _animator.PlayPunchAnimation();
        yield return new WaitUntil(() => _animator.PunchApplyied);
        _enemy.TryApplyDamage(segment.Value);
        TurnEnded?.Invoke();
    }
}
