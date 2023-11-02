using System;
using UnityEngine;

public class JumpEndLine : MonoBehaviour, ILine
{
    [SerializeField] private Transform _destination;
    [SerializeField] private Roulette _roulette;
    [SerializeField] private JumpStartLine _startLine;

    private int _collisionCounter;

    private void OnEnable()
    {
        _startLine.TweenKilled += LineAction;
    }

    private void OnDisable()
    {
        _startLine.TweenKilled -= LineAction;
    }

    public void LineAction(Player player)
    {
        if (++_collisionCounter > 1)
            return;
        Destroy(_roulette.gameObject, 1f);
        player.JumpTo(_destination);
    }
}