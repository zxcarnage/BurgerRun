using UnityEngine;

public class JumpPadLine : MonoBehaviour, ILine
{
    [SerializeField] private Transform _pad;
    public void LineAction(Player player)
    {
        player.PadJumpTo(_pad, false);
    }
}
