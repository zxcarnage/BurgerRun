using System;
using DG.Tweening;
using UnityEngine;

public class FinishLine : MonoBehaviour, ILine
{
    [SerializeField] private Transform _finishLine;
    [SerializeField] private float _animationDuration;
    [SerializeField] private PlayerRuntimeStats _stats;

    public event Action<Player> FinishLineCrossed;
    public Transform LineModel => _finishLine;
    public void LineAction(Player player)
    {
        _finishLine.DOMove(_finishLine.position, _animationDuration);
        player.BlockMovement();
        _stats.SetHealth(player.Health);
        _stats.MaxHealth = player.MaxHealth;
        FinishLineCrossed?.Invoke(player);
    }
}
