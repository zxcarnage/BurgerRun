using UnityEngine;

public class Broccoli : Food, IEatable
{
    [SerializeField] private AudioClip _broccoliEaten;
    public void Eat(Player player)
    {
        player.TryApplyDamage(_foodData.ValueShift);
        //player.PlayerAnimator.SetRunningTrigger();
        FoodEaten();
        SoundManager.Instance.PlaySound(_broccoliEaten);
    }
}
