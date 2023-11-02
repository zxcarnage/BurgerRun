using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Currency Data", menuName = "Player/Player Currency", order = 0)]
public class Currensy : ScriptableObject
{
    [SerializeField] private PlayerRuntimeStats _runtimeStats;
    [SerializeField] private float _coinAmount;
    private float _levelResult;
    
    public float CoinAmount => _coinAmount;
    public float LevelResult => _levelResult;
    public event Action ValueUpdated;

    public void AddMoney(float value)
    {
        if (value <= 0)
            return;
        _coinAmount += value;
        ValueUpdated?.Invoke();
    }

    public void AddMoneyByMultiplyier(float multiplyier)
    {
        if (multiplyier <= 0)
            return;
        _levelResult = Mathf.Clamp(_runtimeStats.Health * multiplyier, 0f , 800) + Random.Range(100, 351);
        AddMoney(_levelResult);
    }

    public bool TryDecreaseMoney(float value)
    {
        if (value < 0)
            return false;
        if (value > _coinAmount)
            return false;
        DecreaseMoney(value);
        return true;
    }

    private void DecreaseMoney(float value)
    {
        _coinAmount -= value;
        ValueUpdated?.Invoke();
    }

    public void SetStartingMoney(float value)
    {
        _coinAmount = value;
    }
    
}
