using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamUI : MonoBehaviour
{
    [SerializeField] private Slider _balanceSlider;
    public Slider BalanceSlider => _balanceSlider;

    public void ChangeSliderValue(float value)
    {
        _balanceSlider.value = value;
    }
}
