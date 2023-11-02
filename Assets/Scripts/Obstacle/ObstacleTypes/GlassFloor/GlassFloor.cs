using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassFloor : MonoBehaviour, IAreaTrigger
{
    [SerializeField] private List<GlassSegment> _segments;

    private float _amountToAffectPlayerSpeed = 200f;

    private Transform _playerTransform;
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public void AffectPlayer(Player player)
    {
        _playerTransform = player.transform;
        StartCoroutine(InGlassAreaRoutine());
    }

    private IEnumerator InGlassAreaRoutine()
    {
        while (_playerTransform.position.z < _collider.bounds.max.z)
        {
            for (int i = _segments.Count - 1; i >= 0; i--)
            {
                if (_segments[i].transform.position.z <= _playerTransform.position.z)
                {
                    _segments[i].Break();
                    _segments.RemoveAt(i);
                }
            }

            yield return null;
        }
    }

    public void EnterArea(Player player)
    {
        player.PlayerAnimator.SetInsideGlass(true);
        _playerTransform = player.transform;
        StartCoroutine(InGlassAreaRoutine());
        player.PlayerMover.ChangeRunningSpeedBy(-_amountToAffectPlayerSpeed);
    }

    public void StayInArea(Player player)
    {
        
    }

    public void ExitArea(Player player)
    {
        player.PlayerAnimator.SetInsideGlass(false);
        player.PlayerMover.ChangeRunningSpeedBy(_amountToAffectPlayerSpeed);
    }
}
