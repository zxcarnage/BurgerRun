using UnityEngine;

public class CheeseDeadZone : MonoBehaviour, IObstacle
{
    [SerializeField] private float _pushPower;
    [SerializeField] private float _damage;
    public void AffectPlayer(Player player)
    {
        player.TryApplyDamage(_damage);
        player.Push(_pushPower);
    }
}
