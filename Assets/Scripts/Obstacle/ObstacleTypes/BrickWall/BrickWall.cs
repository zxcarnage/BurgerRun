using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class BrickWall : MonoBehaviour, IObstacle
{
    [SerializeField] private Transform _dynamicBricks;
    [SerializeField] private Vector3 _pathLength;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _damage = 5;
    [SerializeField] private AudioClip _hitWallSound;

    private Rigidbody[] _rigidbodies;
    private float _areaDelta;

    private void Awake()
    {
        _rigidbodies = _dynamicBricks.GetComponentsInChildren<Rigidbody>();
    }

    public void AffectPlayer(Player player)
    {
        player.BlockMovement(); 
        DOTween.Sequence().Append(player.DoMove(player.transform.position + _pathLength, _moveDuration))
                          .AppendCallback(() => player.UnlockMovement());
        SoundManager.Instance.PlaySound(_hitWallSound);
        player.PlayerAnimator.BreakingWall();
        player.TryApplyDamage(_damage);
        BreakWall(player);
    }

    public void BreakWall(Player player)
    {
        foreach (var brick in _rigidbodies)
        {
            if (Mathf.Abs(brick.position.x - player.transform.position.x) <= 1f)
            {
                brick.AddForce(Vector3.forward * 400);
                Transform[] children = brick.GetComponentsInChildren<Transform>();
                foreach (var child in children)
                    child.AddComponent<Rigidbody>();
            }
        }
    }
}
