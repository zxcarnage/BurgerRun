using UnityEngine;

[RequireComponent(typeof(TwerkPlayerSkinSwitcher))]
public class TwerkPlayerPartIncreaser : ParticipantsPartsIncreaser
{
    private TwerkPlayerSkinSwitcher _skinSwitcher;
    
    private void OnRigChanged(GameObject newRig)
    {
        _meshRenderer = newRig.GetComponentInChildren<SkinnedMeshRenderer>();
    }
    
    protected override void Enable()
    {
        base.Enable();
        _skinSwitcher.RigChanged += OnRigChanged;
    }

    protected override void AwakeVirtual()
    {
        base.AwakeVirtual();
        _skinSwitcher = GetComponent<TwerkPlayerSkinSwitcher>();
    }
    
    protected override void Disable()
    {
        base.Enable();
        _skinSwitcher.RigChanged -= OnRigChanged;
    }
}
