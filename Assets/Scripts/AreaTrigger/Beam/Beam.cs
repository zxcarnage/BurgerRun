using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour, IAreaTrigger
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;

    private bool _isCloseToTheEnd;

    public void EnterArea(Player player)
    {
        player.PlayerMover.MoveTo(_startPosition.position, 0.5f).OnComplete(() =>
        {
            player.PlayerMover.EnterBeam();
            player.PlayerAnimator.SetInsideBeam(true);
        });
        
        
    }

    public void ExitArea(Player player)
    {
        player.PlayerMover.ExitBeam();
        player.PlayerAnimator.SetInsideBeam(false);
    }

    public void StayInArea(Player player)
    {
        if (_isCloseToTheEnd == false && Vector3.Distance(player.transform.position, _endPosition.position) < 3f)
        {
            _isCloseToTheEnd = true;
            player.PlayerMover.BalanceOut();
        }
    }
}
