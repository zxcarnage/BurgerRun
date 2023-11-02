using System;
using DG.Tweening;
using UnityEngine;

public class TextTipRoulette : MonoBehaviour
{
    [SerializeField] private float _animationDuration;
    private void OnEnable()
    {
        transform.DOScale(transform.localScale / 1.2f, _animationDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
