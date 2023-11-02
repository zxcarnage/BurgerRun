using System;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour, IPanel
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private SkinProgression _skinProgression;
    [SerializeField] private Income _income;
    [SerializeField] private Button _nextLvlButton;
    public void Initialize()
    {
        _panel.SetActive(true);
        _income.Init();
        _skinProgression.Init(_income.Currensy.LevelResult);
    }

    private void OnEnable()
    {
        ServiceLocator.LevelSpawner.LevelSpawned += OnLevelSpawned;
        _nextLvlButton.gameObject.SetActive(false);
        Invoke("ActivateNextButton", 2);
    }

    private void OnDisable()
    {
        ServiceLocator.LevelSpawner.LevelSpawned -= OnLevelSpawned;
    }

    private void ActivateNextButton()
    {
        _nextLvlButton.gameObject.SetActive(true);
    }

    private void OnLevelSpawned()
    {
        gameObject.SetActive(false);
    }

    public void BlockButtons()
    {
        _nextLvlButton.interactable = false;
    }

    public void UnlockButtons()
    {
        _nextLvlButton.interactable = true;
    }

    public void Deactivate()
    {
        _panel.SetActive(false);
    }
}
