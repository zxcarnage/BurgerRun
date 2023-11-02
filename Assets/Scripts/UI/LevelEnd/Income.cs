using System;
using TMPro;
using UnityEngine;

public class Income : MonoBehaviour
{
    [SerializeField] private Currensy _currensy;
    [SerializeField] private TMP_Text _incomeText;

    public Currensy Currensy => _currensy;
    public void Init()
    {
        _incomeText.text = "+" + _currensy.LevelResult.ToString("0");
    }
}
