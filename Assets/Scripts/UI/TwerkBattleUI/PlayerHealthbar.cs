using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : Healthbar
{
    [SerializeField] private PlayerSkin _currentSkin;
    [SerializeField] private Image _healthbarPortrait;

    public override void Initialize(float hp, float maxHealth)
    {
        base.Initialize(hp,maxHealth);
        _healthbarPortrait.sprite = _currentSkin.Skin.TwerkPreview;
    }
}
