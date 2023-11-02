using UnityEngine;

public class Fence : MonoBehaviour, IObstacle
{
    [SerializeField] private float _fenceDamage;
    [SerializeField] private float _pushPower;
    [SerializeField] private AudioClip _hitSound;
    public void AffectPlayer(Player player)
    {
        player.TryApplyDamage(_fenceDamage);
        player.Push(_pushPower);
        SoundManager.Instance.PlaySound(_hitSound);
    }
}
