using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InternationalTipChanger : MonoBehaviour
{
    [SerializeField] private Image _imageToChange;
    [SerializeField] private Sprite _ru;
    [SerializeField] private Sprite _en;

    private void Awake()
    {
        _imageToChange.sprite = DataManager.Instance.Language == "ru" ? _ru : _en;
    }
}
