using System;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] private StartPanel _startPanel;
    [SerializeField] private BackButton _backButton;

    private void OnEnable()
    {
        _backButton.BackButtonClicked += OnBackButtonClicked;
    }

    private void OnDisable()
    {
        _backButton.BackButtonClicked -= OnBackButtonClicked;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _startPanel.Show();
    }

    private void OnBackButtonClicked()
    {
        _startPanel.Show();
        Hide();
    }
}
