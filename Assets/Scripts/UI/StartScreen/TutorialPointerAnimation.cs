using System;
using DG.Tweening;
using UnityEngine;

public class TutorialPointerAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _pointer;
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _duration;

    private void OnEnable()
    {
        Sequence infiniteSequence = DOTween.Sequence().SetLoops(-1).SetEase(Ease.Linear);
        infiniteSequence.Append(_pointer.DOMove(_points[1].position, _duration).From(_points[0].position));
        infiniteSequence.Append(_pointer.DOMove(_points[0].position, _duration).From(_points[1].position));
    }
}
