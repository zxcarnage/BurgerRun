using UnityEngine;

[CreateAssetMenu(fileName = "Runtime", menuName = "Player stats/Runtime", order = 0)]
public class PlayerRuntimeStats : ScriptableObject
{
    public float MaxHealth;
    [SerializeField] private float _health;
    public float Health => _health;

    public void AddHealth(float value)
    {
        _health += value;
    }

    public void ReduceHealth(float value)
    {
        _health -= value;
    }

    public void SetHealth(float value)
    {
        _health = value;
    }
}