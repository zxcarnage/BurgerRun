using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAreaTrigger : MonoBehaviour, IAreaTrigger
{
    public void EnterArea(Player player)
    {
        Debug.Log("Player entered slide");
        player.PlayerMover.YVelocityOffset = -7f;
    }

    public void ExitArea(Player player)
    {
        Debug.Log("Player exit slide");
        player.PlayerMover.YVelocityOffset = 0;
    }

    public void StayInArea(Player player)
    {
        
    }
}
