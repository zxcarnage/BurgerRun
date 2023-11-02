using System;
using UnityEngine;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour, IPanel
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _reloadButton;

    private void OnEnable()
    {
        _reloadButton.onClick.AddListener(OnReloadButtonClicked);
    }

    private void OnDisable()
    {
        _reloadButton.onClick.RemoveListener(OnReloadButtonClicked);
    }

    public void Initialize()
    {
        _panel.SetActive(true);
    }

    public void Deactivate()
    {
        _panel.SetActive(false);
    }

    private void OnReloadButtonClicked()
    {
        ServiceLocator.LevelSpawner.Restart();
        Deactivate();
    }
}
