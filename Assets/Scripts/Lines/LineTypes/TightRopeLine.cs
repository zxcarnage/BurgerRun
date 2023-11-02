using System;
using DG.Tweening;
using UnityEngine;

public class TightRopeLine : MonoBehaviour,ILine
{
    [SerializeField] private TightRope _rope;
    [SerializeField] private Transform _startPostion;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _time;

    public event Action<Player> PlayerEntered;
    private Player _player;
    
    public void LineAction(Player player)
    {
        _player = player;
        PlayerEntered?.Invoke(player);
        player.GetComponent<PlayerMover>().Block(true);
        player.DoMove(_startPostion.position, _time).OnComplete(EnableRopeMovement);
    }

    private void EnableRopeMovement()
    {
        var ropePlayerMover = _player.GetComponent<TightRopePlayerMover>();
        ropePlayerMover.GiveTightRope(_rope);
        ropePlayerMover.GiveDestination(_endPosition);
        ropePlayerMover.enabled = true;
        
    }
}
