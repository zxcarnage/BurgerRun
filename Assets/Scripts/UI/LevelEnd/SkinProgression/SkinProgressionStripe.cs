using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinProgressionStripe : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _animationDuration;
    private float _currentFullfillPercent;

    public void Init(float fullfillPercent)
    {
        StartCoroutine(AnimateSlider(_currentFullfillPercent + fullfillPercent));
    }

    private IEnumerator AnimateSlider(float targetValue)
    {
        float elapsedTime = 0f;
        var targetValueClamped = Mathf.Clamp(targetValue, 0f, 1f);
        while (elapsedTime <= _animationDuration)
        {
            var currentValue =
                Mathf.Lerp(_currentFullfillPercent, targetValueClamped, elapsedTime / _animationDuration);
            _slider.value = currentValue;
            _text.text = (currentValue * 100).ToString("0") + "%";
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _text.text = (targetValueClamped * 100).ToString("0") + "%";
        _slider.value = targetValueClamped;
        _currentFullfillPercent = targetValueClamped;
    }

    public void Reset()
    {
        _slider.value = 0f;
        _currentFullfillPercent = 0f;
    }
}
