using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player in game Skin", menuName = "Skin/Player in game", order = 0)]
public class PlayerSkin : ScriptableObject
{
    [SerializeField] private Skin _skin;
    
    public Skin Skin => _skin;

    public event Action<Skin> SkinChanged;

    public void ChangeSkin(Skin newSkin)
    {
        _skin = newSkin;
        DataManager.Instance.SaveData();
        SkinChanged?.Invoke(newSkin);
    }
}