using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    private SoundState _state;

    private void Start()
    {
        Debug.Log("_state" + _state);
        _state = AudioListener.pause? SoundState.Off : SoundState.On;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnSoundButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnSoundButtonClicked);
    }

    private void OnSoundButtonClicked()
    {
        if (_state == SoundState.On)
        {
            _state = SoundState.Off;
            _image.sprite = _soundOff;
            AudioListener.pause = true;
        }
        else
        {
            _state = SoundState.On;
            _image.sprite = _soundOn;
            AudioListener.pause = false;
        }
    }
    
    private enum SoundState
    {
        On,
        Off
    }
}

