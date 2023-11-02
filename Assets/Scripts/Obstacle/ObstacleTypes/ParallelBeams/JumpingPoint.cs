using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPoint : MonoBehaviour, IObstacle
{
    [SerializeField] private Transform _destination;
    [SerializeField] private ParallelLanes _currentLane;

    public void AffectPlayer(Player player)
    {
        player.PlayerAnimator.AnimateJumpToBeams();
        DOTween.Sequence().Append(player.PlayerMover.RestrictedJump(_destination, 2, 1f)).SetEase(Ease.Linear)
                          .AppendCallback(() => player.PlayerRollingMovement.StartMovement(_currentLane));
        
    }
}
