using UnityEngine;

public class UphillAreaTrigger : MonoBehaviour, IAreaTrigger
{
    public void EnterArea(Player player)
    {
        player.PlayerMover.YVelocityOffset = 7f;
    }

    public void StayInArea(Player player)
    {
        
    }

    public void ExitArea(Player player)
    {
        player.PlayerMover.YVelocityOffset = 0f;
    }
}
