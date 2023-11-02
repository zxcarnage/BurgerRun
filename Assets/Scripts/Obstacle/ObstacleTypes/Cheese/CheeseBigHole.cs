using DG.Tweening;
using UnityEngine;

public class CheeseBigHole : MonoBehaviour, IObstacle
{
    [SerializeField] private Transform _bigHoleDestiation;
    [SerializeField] private Transform _deadZone;
    [SerializeField] private float _duration;
    public void AffectPlayer(Player player)
    {
        _deadZone.gameObject.SetActive(false);
        player.BlockMovement();
        HoleJump(player);
    }

    private void HoleJump(Player player)
    {
        player.PlayerAnimator.PlayCheeseAnimation();
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.DoMove(_bigHoleDestiation.position, _duration).SetEase(Ease.Linear).OnComplete(player.PlayerAnimator.UnlockMovement);
    }
}
