using System;
using UnityEngine;
using UnityEngine.UI;

public class TwerkBattleUI : MonoBehaviour
{
    [SerializeField] private Participant[] _participants;
    [SerializeField] private AudioClip _hitSound;

    public void Show()
    {
        InitializeHealthbars();
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        foreach (var participant in _participants)
        {
            participant.HealthChanged += OnParticipantHealthChanged;
        }
    }

    private void OnDisable()
    {
        foreach (var participant in _participants)
        {
            participant.HealthChanged -= OnParticipantHealthChanged;
        }
    }

    private void InitializeHealthbars()
    {
        foreach (var participant in _participants)
        {
            participant.InitializeHealthbars();
        }
    }

    private void OnParticipantHealthChanged(Participant participant)
    {
        participant.ChangeHealthbarValue();
        SoundManager.Instance.PlaySound(_hitSound);
    }
}
