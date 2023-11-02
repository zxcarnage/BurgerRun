using UnityEngine;

public class PushRouletteSegment : RouletteSegment
{
    [SerializeField] private float _multiplyier;

    private void OnEnable()
    {
        _value = _multiplyier;
    }
}
