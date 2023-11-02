using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFlame : MonoBehaviour
{
    [SerializeField] private Image _flameImage;
    [SerializeField] private Sprite _inactiveFlame;
    [SerializeField] private Sprite _activeFlame;

    public void OnValueChanged(float value)
    {
        if (value >= 1)
        {
            _flameImage.sprite = _activeFlame;
            _flameImage.transform.DOShakeScale(1, 0.15f, 3).SetLoops(-1);
            _flameImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
        else
        {
            _flameImage.sprite = _inactiveFlame;
            _flameImage.transform.localScale = new Vector3(1, 1, 1);
            _flameImage.DOKill();
        }
    }

    private void Start()
    {
        
    }
}
