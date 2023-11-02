using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Player/Player Stats", order = 0)]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private float _startingDistance = 6.8f;
    [SerializeField] private float _startingPushStrength = 69f;
    [SerializeField] private float _maxDistance;
    [SerializeField] private bool _noAds;
    [SerializeField] private int _moneyProgression;
    public float StartingDistance => _startingDistance;
    public int MoneyProgression => _moneyProgression;
    public float StartingPushStrength => _startingPushStrength;

    public float PushStrength;
    public float MaxDistance => _maxDistance;
    public bool NoAds => _noAds;
    public event Action DistanceChanged;

    public void IncreaseDistance(float amount)
    {
        if (amount <= 0)
            return;
        _maxDistance += amount;
        _maxDistance = MathF.Round(_maxDistance, 1);
        DistanceChanged?.Invoke();
    }

    public void IncreasePushStrength(float amount)
    {
        if (amount <= 0)
            return;
        PushStrength += amount;
        PushStrength = MathF.Round(PushStrength, 1);
    }

    public void SetMaxDistance(float amount)
    {
        _maxDistance = amount;
        _maxDistance = MathF.Round(_maxDistance, 1);
    }
    
    public void SetPushStrenght(float amount)
    {
        PushStrength = amount;
        PushStrength = MathF.Round(PushStrength, 1);
    }

    public void SetNoAds(bool noAds)
    {
        _noAds = noAds;
    }

    public void SetMoneyProgression(int index)
    {
        _moneyProgression = index;
    }
}