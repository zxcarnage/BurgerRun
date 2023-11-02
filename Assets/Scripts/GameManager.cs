using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Agava.WebUtility;
using Agava.YandexGames;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Slider _slider;

    [field:SerializeField] public float ButtSize { get; private set; } = 0;
    

    private void Awake()
    {
#if !UNITY_EDITOR
        GameReady.NotifyLoadingCompleted();
#endif
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += InBackground;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= InBackground;
    }

    private void InBackground(bool isInBackground)
    {
        Time.timeScale = Convert.ToInt16(!isInBackground);
        Debug.Log("Time.timeScale" + Time.timeScale);
        AudioListener.pause = isInBackground;
        Debug.Log("Volume" + AudioListener.pause);
    }
    
    public void ChangeSliderValue(float value)
    {
        _slider.DOKill();
        _slider.DOValue(value, 0.25f).SetEase(Ease.InOutSine);
        ButtSize = value;
    }
    

    public void ResetSlider()
    {
        _slider.DOKill();
        _slider.value = 0f;
    }

    public void DisableSlider()
    {
        ChangeSliderState(false);
    }

    public void EnableSlider()
    {
        ChangeSliderState(true);
    }

    private void ChangeSliderState(bool state)
    {
        _slider.transform.parent.gameObject.SetActive(state);
    }
}
