using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxItemCoin : BoxItem
{
    [SerializeField] private Text _coinsText;
    [SerializeField] private Currensy _currensy;

    private int _coinReward;

    public void SetUpBox(int reward)
    {
        _coinReward = reward;
        _coinsText.text = reward.ToString();
    }

    public override void OpenBox()
    {
        base.OpenBox();
        IncreaseMoney();
    }

    private void IncreaseMoney()
    {
        _currensy.AddMoney(_coinReward);
    }
}

