using System;
using Agava.YandexGames;
using UnityEngine;

public class InterstitialAdsManager : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;

    private void Awake()
    {
        ServiceLocator.AdsManager = this;
    }

    private void Start()
    {
        var isPaused = AudioListener.pause;
        PreAdScreen.Instance.ShowAdClicker();
        //TryShowAd(() => AudioListener.pause = true, (x) => AudioListener.pause = isPaused);
    }

    public void TryShowAd(Action onOpenCallback = null, Action<bool> onCloseCallback = null)
    {
#if UNITY_EDITOR
        return;
#endif
        if (_playerStats.NoAds == false)
        {
            ShowAd(onOpenCallback, onCloseCallback);
        }
    }

    public bool CanShowAd()
    {
        if (_playerStats.NoAds == false)
            return true;
        else
            return false;
    }

    private void ShowAd(Action onOpenCallback = null,Action<bool> onCloseCallback = null)
    {
        InterstitialAd.Show(onOpenCallback,onCloseCallback);
    }
}
