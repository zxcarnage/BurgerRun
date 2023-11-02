using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingTrigger : MonoBehaviour, IObstacle
{
    [SerializeField] private ParallelBeams _beams;
    public void AffectPlayer(Player player)
    {
        player.PlayerAnimator.SetRollingTrigger();
        player.PlayerRollingMovement.SetUpCollider();
        player.PlayerRollingMovement.SetRotationForMovement(transform.eulerAngles.x);
        player.PlayerRollingMovement.SetBeams(_beams);
    }
}
