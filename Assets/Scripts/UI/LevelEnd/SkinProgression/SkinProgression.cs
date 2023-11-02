using System;
using Agava.YandexGames;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkinProgression : MonoBehaviour
{
    [SerializeField] private Rewards _rewards;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private SkinProgressionModel _model;
    [SerializeField] private SkinProgressionStripe _stripe;
    private float _fullfillPercent;
    
    public void Init(float income)
    {
        var currentFullfill = MathF.Round(income / 10000, 1);
        currentFullfill = Mathf.Clamp(currentFullfill, 0, 1f);
        currentFullfill = 0.1f; // Hard coded to 0.1f. Can be changed
        _fullfillPercent += currentFullfill;
        _fullfillPercent = Mathf.Clamp(_fullfillPercent, 0, 1f);
        _model.Init(currentFullfill, ModelInitialized);
        if(_model.enabled)
            _stripe.Init(currentFullfill);
        else
            _stripe.gameObject.SetActive(false);
    }

    private void ModelInitialized()
    {
        if (_fullfillPercent >= 1f)
        {
            _model.Animate();
            SuggestToTakeReward();
        }
    }

    private void OnEnable()
    {
        _rewards.GetRewardsButton.onClick.AddListener(TakeReward);
        _rewards.LoseRewardsButton.onClick.AddListener(LoseReward);
    }

    private void OnDisable()
    {
        _rewards.GetRewardsButton.onClick.RemoveListener(TakeReward);
        _rewards.LoseRewardsButton.onClick.RemoveListener(LoseReward);
    }

    private void SuggestToTakeReward()
    {
        SetSuggestionState(true);
    }

    private void TakeReward()
    {
        YandexManager.Instance.WatchRewardedVideo(OpenNewSkinAndLoadNextLevel);
    }

    private void OpenNewSkinAndLoadNextLevel()
    {
        _model.OpenNewSkin();
        ResetProgress();
        ServiceLocator.LevelSpawner.LoadNext();
    }

    private void ResetProgress()
    {
        _model.ResetSkinProgression();
        _stripe.Reset();
        _fullfillPercent = 0f;
        SetSuggestionState(false);
    }

    private void SetSuggestionState(bool state)
    {
        _nextLevelButton.gameObject.SetActive(!state);
        _rewards.gameObject.SetActive(state);
    }

    private void LoseReward()
    {
        ResetProgress();
        ServiceLocator.LevelSpawner.LoadNext();
    }
}
