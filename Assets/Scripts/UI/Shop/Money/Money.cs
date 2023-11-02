using System;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsAmountText;
    [SerializeField] private Currensy _coinsAmount;

    private void OnEnable()
    {
        _coinsAmount.ValueUpdated += OnValueUpdated;
        OnValueUpdated();
    }

    private void OnDisable()
    {
        _coinsAmount.ValueUpdated -= OnValueUpdated;
    }

    private void OnValueUpdated()
    {
        _coinsAmountText.text = _coinsAmount.CoinAmount.ToString("0");
    }
}
