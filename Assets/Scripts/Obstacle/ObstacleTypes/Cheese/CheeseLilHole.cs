using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CheeseLilHole : MonoBehaviour, IObstacle
{
    [SerializeField] private Transform _destination;
    [SerializeField] private Transform _deadZone;
    [SerializeField] private Transform _stuckDestination;
    [SerializeField] private float _healthBorder;
    [SerializeField] private float _duration;

    private Sequence _moveSequence;
    public void AffectPlayer(Player player)
    {
        _deadZone.gameObject.SetActive(false);
        player.BlockMovement();
        player.PlayerAnimator.PlayCheeseAnimation();
        DoHoleJump(player);
        StartCoroutine(TryStuck(player));
    }

    private void DoHoleJump(Player player)
    {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _moveSequence = DOTween.Sequence();
        _moveSequence.Append(player.DoMove(_destination.position, _duration).OnComplete(() => player.PlayerAnimator.UnlockMovement()));
    }

    private IEnumerator TryStuck(Player player)
    {
        if (player.Health >= _healthBorder)
        {
            while (player.transform.position.z < _stuckDestination.position.z)
            {
                yield return null;
            }

            Stuck(player);
        }
    }

    private void Stuck(Player player)
    {
        _moveSequence.Kill();
        player.transform.position = _stuckDestination.position;
        Debug.Log(new Vector3(_stuckDestination.position.x, transform.position.y, _stuckDestination.position.z));
        player.PlayerAnimator.StuckAnimation();
        StartCoroutine(DieAfter(player, 1.5f));
    }

    private IEnumerator DieAfter (Player player, float duration)
    {
        yield return new WaitForSeconds(duration);
        player.Die();
    }
}


