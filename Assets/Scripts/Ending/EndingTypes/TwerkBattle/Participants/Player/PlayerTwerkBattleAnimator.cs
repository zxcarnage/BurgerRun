
using UnityEngine;

[RequireComponent(typeof(TwerkPlayerSkinSwitcher))]
public class PlayerTwerkBattleAnimator : ParticipantTwerkBattleAnimator
{
    private TwerkPlayerSkinSwitcher _switcher;
    private void Awake()
    {
        _switcher = GetComponent<TwerkPlayerSkinSwitcher>();
    }

    private void OnEnable()
    {
        _switcher.RigChanged += OnRigChanged;
    }

    private void OnDisable()
    {
        _switcher.RigChanged -= OnRigChanged;
    }
    
    private void OnRigChanged(GameObject newRig)
    {
        _animator = newRig.GetComponent<Animator>();
    }
}
