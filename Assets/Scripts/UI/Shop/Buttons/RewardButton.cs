using System;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int _reward;
    [SerializeField] private Currensy _currensy;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnRewardButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnRewardButtonClick);
    }

    private void OnRewardButtonClick()
    {
        YandexManager.Instance.WatchRewardedVideo(IncreaseMoney);
    }

    private void IncreaseMoney()
    {
        _currensy.AddMoney(_reward);
    }
}
