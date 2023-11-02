using System;
using UnityEngine;

[RequireComponent(typeof(Participant))]
public abstract class ParticipantsPartsIncreaser : MonoBehaviour
{
    [SerializeField] private float _maxBlendShapeValue = 70f;
    [SerializeField] protected SkinnedMeshRenderer _meshRenderer;

    private Participant _participant;

    private void Awake()
    {
        AwakeVirtual();
    }

    private void OnEnable()
    {
        Enable();
    }

    private void OnDisable()
    {
        Disable();
    }

    protected virtual void Enable()
    {
        _participant.HealthChanged += OnHealthChanged;
    }

    protected virtual void AwakeVirtual()
    {
        _participant = GetComponent<Participant>();
    }

    protected virtual void Disable()
    {
        _participant.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(Participant participant)
    {
        if (participant.Health < 0)
            return;
        var index = _meshRenderer.sharedMesh.blendShapeCount == 1? 0 : 1;
        _meshRenderer.SetBlendShapeWeight(index, _maxBlendShapeValue * participant.Health / participant.MaxHealth);

        GameManager.Instance.ChangeSliderValue(Mathf.Lerp(0, 1, participant.Health / participant.MaxHealth));
    }
}
