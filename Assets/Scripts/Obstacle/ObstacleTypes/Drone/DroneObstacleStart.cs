using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneObstacleStart : MonoBehaviour, IObstacle
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Drone _drone;

    public void AffectPlayer(Player player)
    {
        StartCoroutine(MountAnimation(player));
    }

    private IEnumerator MountAnimation(Player player)
    {
        player.PlayerMover.Block(true);
        player.PlayerMover.MoveTo(_startPosition.position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Camera.Instance.UpdateOffsetBy(new Vector3(0, 1.5f, 0), 2);
        Camera.Instance.UpdateRotationBy(new Vector3(10, 0, 0), 2);
        player.PlayerAnimator.MountDrone();
        yield return WaitFor.Frames(15);
        player.transform.DOLocalMoveY(player.transform.localPosition.y + 1.35f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        _drone.SetHandsTrigger(player);
        _drone.transform.SetParent(player.transform);
        player.PlayerMover.Block(false);
        player.PlayerAnimator.AnimateDroneMovement();
    }
}
