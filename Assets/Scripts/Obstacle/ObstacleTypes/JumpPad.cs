using System.Collections;
using UnityEngine;

public class JumpPad : MonoBehaviour, IObstacle
{
    [SerializeField] private Transform _jumpFinalPoint;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _padSound;

    public void AffectPlayer(Player player)
    {
        StartCoroutine(PlayAnimation());
        player.PlayerAnimator.GroundedOnPad();
        player.PadJumpTo(_jumpFinalPoint.transform, true);
        SoundManager.Instance.PlaySound(_padSound);
    }

    private IEnumerator PlayAnimation()
    {
        yield return WaitFor.Frames(5);
        _animator.SetTrigger("Play");
    }
}
