using UnityEngine;

[CreateAssetMenu(fileName = "New Food Data", menuName = "Food/Food Data", order = 0)]
public class FoodData : ScriptableObject
{
    [SerializeField] private float _valueShift;

    public float ValueShift => _valueShift;
}
