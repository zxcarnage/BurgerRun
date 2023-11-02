using System;
using DG.Tweening;
using UnityEngine;

public class JumpStartLine : MonoBehaviour, ILine
{
    [SerializeField] private Transform _miniGameDestination;
    [SerializeField] private float _movementDuration;
    [SerializeField] private Roulette _roulette;
    [SerializeField] private float _roulletteMultiplier = 20f;

    private Tween _tween;
    private Player _player;
    public event Action<Player> TweenKilled;

    private void OnEnable()
    {
        _roulette.SegmentChosen += OnSegmentChosen;
    }

    private void OnDisable()
    {
        _roulette.SegmentChosen -= OnSegmentChosen;
    }

    public void LineAction(Player player)
    {
        var destination = new Vector3(_miniGameDestination.position.x, player.transform.position.y,
            _miniGameDestination.position.z);
        _player = player;
        _player.PlayerAnimator.Slow(0.3f);
        _tween = player.DoMove(destination,_movementDuration).OnComplete(() => _player.PlayerAnimator.UnSlow()).OnKill(() => _player.PlayerAnimator.UnSlow());
        _roulette.gameObject.SetActive(true);
        GameManager.Instance.DisableSlider();
    }

    private void OnSegmentChosen(RouletteSegment segment)
    {
        _player.GiveAdditionalPower(segment.Value * _roulletteMultiplier);
        _roulette.gameObject.SetActive(false);
        _tween.Kill();
        TweenKilled?.Invoke(_player);
    }
}
