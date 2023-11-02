using System;
using TMPro;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    [SerializeField] private RouletteArrow _arrow;
    [SerializeField] private string _additionalText;
    [SerializeField] private TMP_Text _valueText;
    public event Action<RouletteSegment> SegmentChosen;

    private void OnEnable()
    {
        _arrow.SegmentCrossed += ShowSegmentInfo;
        _arrow.ArrowStopped += OnArrowStopped;
    }

    private void OnDisable()
    {
        _arrow.SegmentCrossed -= ShowSegmentInfo;
        _arrow.ArrowStopped -= OnArrowStopped;
    }

    private void OnArrowStopped(RouletteSegment segment)
    {
        SegmentChosen?.Invoke(segment);
    }

    private void ShowSegmentInfo(RouletteSegment segment)
    {
        _valueText.text = _additionalText + Convert.ToString(segment.Value);
    }
}
