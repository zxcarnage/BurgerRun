using Agava.YandexGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YandexManager : MonoBehaviour
{
    public static YandexManager Instance { get; private set; }
    public bool AdsReady { get; private set; }

    [SerializeField] private float _timeToShowAd = 60f;
    [SerializeField] private PlayerStats _playerStats;

    private float _timer = 100;
    private bool _isPaused;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    public void TryShowAd(Action onOpenCallback = null, Action<bool> onCloseCallback = null)
    {
#if UNITY_EDITOR
        return;
#endif
        if (_playerStats.NoAds == false && _timer >= _timeToShowAd)
        {
            _timer = 0;
            ShowAd(onOpenCallback, onCloseCallback);
        }
    }

    public bool CanShowAd()
    {
        if (_playerStats.NoAds == false && _timer >= _timeToShowAd)
            return true;
        else
            return false;
    }

    private void ShowAd(Action onOpenCallback = null, Action<bool> onCloseCallback = null)
    {
        InterstitialAd.Show(onOpenCallback, onCloseCallback);
    }

    public void ShowAdWithCallbacks()
    {
        Debug.Log("Showing Ad");
        _timer = 0;
#if UNITY_EDITOR
        return;
#endif
        InterstitialAd.Show(OnAdOpened,(data) => OnAdClosed());
    }

    public void WatchRewardedVideo(Action action)
    {
        Debug.Log("Showing rewarded video");
#if UNITY_WEBGL && !UNITY_EDITOR
        VideoAd.Show(OnAdOpened, action, OnAdClosed, (data) => OnAdClosed());
        return;
#endif

#if !UNITY_WEBGL || UNITY_EDITOR
        OnAdOpened();
        OnAdClosed();
        action.Invoke();
#endif
    }

    private void OnAdClosed()
    {
        AudioListener.pause = _isPaused;
        Debug.Log("On ad closed + " + _isPaused);
        AudioListener.volume = 1;
        Time.timeScale = 1;
    }

    private void OnAdOpened()
    {
        _isPaused = AudioListener.pause;
        Debug.Log("On ad opened + " + _isPaused);
        AudioListener.pause = true;
        AudioListener.volume = 0;
        Time.timeScale = 0;
    }
}
