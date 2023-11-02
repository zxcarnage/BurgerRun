using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitFall : MonoBehaviour, IObstacle
{
    public void AffectPlayer(Player player)
    {
        player.FallInToPit();
    }
}
