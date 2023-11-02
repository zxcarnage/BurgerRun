using DG.Tweening;
using UnityEngine;

public class Burger : Food, IEatable
{
    [SerializeField] private AudioClip _burgerEaten;
    public void Eat(Player player)
    {
        player.TryApplyDamage(-_foodData.ValueShift);
        //player.PlayerAnimator.SetRunningTrigger();
        FoodEaten();
        SoundManager.Instance.PlaySound(_burgerEaten);
    }
}
