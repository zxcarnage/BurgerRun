using DG.Tweening;
using UnityEngine;

public class BlockMovementLine : MonoBehaviour, ILine
{
    [SerializeField] private Transform _miniGameStartLine;
    [SerializeField] private float _movementDuration;
    public void LineAction(Player player)
    {
        player.BlockMovement();
        var destination = new Vector3(_miniGameStartLine.position.x, player.transform.position.y,
            _miniGameStartLine.position.z);
        player.DoMove(destination, _movementDuration);
    }
}
