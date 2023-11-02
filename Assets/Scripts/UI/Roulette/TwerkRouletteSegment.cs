using System;
using UnityEngine;

public class TwerkRouletteSegment : RouletteSegment
{
    [SerializeField] private int _segmentNumber;
    [SerializeField] private PlayerRuntimeStats _playerRuntimeStats;

    private void Start()
    {
        CountValue();
    }

    private void CountValue()
    {
        _value = Convert.ToInt16(_playerRuntimeStats.Health / (_segmentNumber + 1));
        
    }
}
