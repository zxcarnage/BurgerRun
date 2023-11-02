using UnityEngine;

public class TrampolineDeadZone : MonoBehaviour, IObstacle
{
    public void AffectPlayer(Player player)
    {
        player.FallInToPool();
    }
}
