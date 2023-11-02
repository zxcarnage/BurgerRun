using System;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerPartsIncreaser : MonoBehaviour
{
    [SerializeField] private float _maxBlendShapeValue = 70f;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    
    private Player _player;
    private PlayerSkinSwitcher _skinSwitcher;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _skinSwitcher = GetComponent<PlayerSkinSwitcher>();
    }

    private void OnEnable()
    {
        _player.HealthChanged += OnHealthChanged;
        _skinSwitcher.RigChanged += OnRigChanged;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= OnHealthChanged;
        _skinSwitcher.RigChanged -= OnRigChanged;
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        var index = _meshRenderer.sharedMesh.blendShapeCount == 1? 0 : 1;
        _meshRenderer.SetBlendShapeWeight(index, _maxBlendShapeValue * currentHealth / maxHealth);

        GameManager.Instance.ChangeSliderValue(Mathf.Lerp(0, 1, currentHealth / maxHealth));
    }

    private void OnRigChanged(GameObject newRig)
    {
        _meshRenderer = newRig.GetComponentInChildren<SkinnedMeshRenderer>();
    }
}
