using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigUphillArea : MonoBehaviour, IAreaTrigger
{
    [SerializeField] private float _newRadius = 0.2f;
    [SerializeField] private float _increaseSpeedBy = 200f;
    private float _oldRadius;
    private CapsuleCollider _collider;

    private void Start()
    {
        
    }
    public void EnterArea(Player player)
    {
        _collider = player.PlayerAnimator.GetComponent<CapsuleCollider>();
        _oldRadius = _collider.radius;
        _collider.radius = _newRadius;
        player.PlayerMover.ChangeRunningSpeedBy(_increaseSpeedBy);
    }

    public void ExitArea(Player player)
    {
        _collider.radius = _oldRadius;
        player.PlayerMover.ChangeRunningSpeedBy(-_increaseSpeedBy);
    }

    public void StayInArea(Player player)
    {
        
    }
}
