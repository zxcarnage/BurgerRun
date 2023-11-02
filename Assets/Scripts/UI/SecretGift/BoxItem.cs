using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxItem : MonoBehaviour
{
    public bool OpenedBox => _openedBox;
    public Action BoxOpened;

    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _boxAnimator;
    [SerializeField] private Button _mainButton;
    [SerializeField] private GameObject _bonus;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private GameObject _openSuitcase;
    [SerializeField] private AudioClip _openBoxSound;
    
    private const string Open = "Open";
    private const string Play = "Play";
    private const string Reset = "Reset";

    private bool _openedBox;


    private void Start()
    {
        _mainButton.onClick.AddListener(OpenBox);
        _rewardButton.onClick.AddListener(WatchRewarded);
    }

    public virtual void OpenBox()
    {
        if (_openedBox) return;

        _animator.SetTrigger(Open);
        _bonus.gameObject.SetActive(true);
        _openedBox = true;
        _boxAnimator.SetTrigger(Reset);
        BoxOpened?.Invoke();
        SoundManager.Instance.PlaySound(_openBoxSound);
    }

    public void ActivateRewardButton()
    {
        _mainButton.onClick.RemoveAllListeners();
        _boxAnimator.SetTrigger(Play);
        _rewardButton.gameObject.SetActive(true);
    }

    private void WatchRewarded()
    {
        YandexManager.Instance.WatchRewardedVideo(OpenBox);
    }
}
