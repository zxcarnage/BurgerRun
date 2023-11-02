using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Healthbar : MonoBehaviour
{
    [SerializeField] private TMP_Text _value;
    [SerializeField] private Slider _healthSlider;

    public void ChangeHealth(float actualHealth, float maxHealth)
    {
        _value.text = actualHealth.ToString("N0");
        _healthSlider.value = Mathf.Lerp(0, 1, actualHealth/maxHealth);
    }

    public virtual void Initialize(float hp, float maxHealth)
    {
        ChangeHealth(hp, maxHealth);
    }
}
