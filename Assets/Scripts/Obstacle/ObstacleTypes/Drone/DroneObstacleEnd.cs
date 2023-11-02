using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneObstacleEnd : MonoBehaviour,IObstacle
{
    [SerializeField] private Transform _endPosition;
    [SerializeField] private GameObject _drone;

    public void AffectPlayer(Player player)
    {
        player.PlayerMover.Block(true);
        Camera.Instance.UpdateRotationBy(Vector3.zero, 2);
        Camera.Instance.UpdateOffsetBy(Vector3.zero, 2);
        DOTween.Sequence().Append(player.transform.DOMove(_endPosition.position, 1f))
                          .AppendCallback(player.PlayerAnimator.DismountDrone)
                          .Append(_drone.transform.DOLocalMoveY(10, 1).OnComplete(() => Destroy(_drone)));
    }
}
