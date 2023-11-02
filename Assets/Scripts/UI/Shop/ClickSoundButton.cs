using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSoundButton : MonoBehaviour
{
    [SerializeField] private AudioClip _clickSound;

    private Button _button;

    private void Awake()
    {
        if (TryGetComponent(out Button button))
        {
            _button = button;
            _button.onClick.AddListener(ClickSound);
        }
    }

    private void ClickSound()
    {
        SoundManager.Instance.PlaySound(_clickSound);
    }
}
