using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEndTrigger : MonoBehaviour, IObstacle
{
    [SerializeField] private Transform _destination;

    public void AffectPlayer(Player player)
    {
        player.PlayerRollingMovement.StopMovement();
        DOTween.Sequence().Append(player.PlayerMover.RestrictedJump(_destination, 4, 1))
                          .Join(player.transform.DOLocalRotate(new Vector3(720,0,0), 1))
                          .AppendCallback(player.PlayerMover.Land);
    }
}
