using System;
using UnityEngine;

public class TightRope : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private TightRopeLine _line;
    [SerializeField] private Indicator _indicator;
    
    private PlayerInput _playerInput;

    private float _value;

    public event Action<float> PlayerFalls;
    
    public float Value => _value;

    private void OnEnable()
    {
        _line.PlayerEntered += OnPlayerEntered;
    }

    private void OnDisable()
    {
        _line.PlayerEntered -= OnPlayerEntered;
        _indicator.gameObject.SetActive(false);
    }

    private void OnPlayerEntered(Player player)
    {
        _playerInput = player.GetComponent<PlayerInput>();
        _indicator.gameObject.SetActive(true);
    }
    
    private void Update()
    {
        if (_playerInput == null)
            return;
        _value += (_playerInput.GetXDirection() + 0.1f * Mathf.Sign(_value)) * _speed * Time.deltaTime;
        Debug.Log(_value);
        if (_value is < -1 or > 1)
            PlayerFalls?.Invoke(_value);
    }
}
