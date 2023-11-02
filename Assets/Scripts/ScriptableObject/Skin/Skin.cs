using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Skin", menuName = "Skin/Common Skin", order = 0)]
public class Skin : ScriptableObject
{
    [SerializeField] private GameObject _skinRig;
    [SerializeField] private Sprite _skinPreview;
    [SerializeField] private Sprite _battlePreview;
    [SerializeField] private PlayerSkin _playerCurrent;
    [Tooltip("Skin Meshes")]
    [SerializeField] private int _hairCounter;
    [SerializeField] private Material _hairMaterial;
    [SerializeField] private Material _body;
    [SerializeField] private int _boots;
    [SerializeField] private Material _bootsMaterial;
    [SerializeField] private SkinState _state = SkinState.Closed;
    
    public Sprite SkinPreview => _skinPreview;
    public Sprite TwerkPreview => _battlePreview;
    public int HairCounter => _hairCounter;
    public Material HairMaterial => _hairMaterial;
    public Material Body => _body;
    public int BootsNum => _boots;
    public Material BootsMaterial => _bootsMaterial;
    public GameObject SkinRig => _skinRig;

    public bool Chosen()
    {
        return this == _playerCurrent.Skin;
    }
}
