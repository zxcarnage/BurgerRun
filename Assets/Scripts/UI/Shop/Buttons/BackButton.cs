using System;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action BackButtonClicked;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnBackButtonClicked);
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
    }
}
