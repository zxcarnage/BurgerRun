using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _maxRotation;

    private readonly static int InHands = Animator.StringToHash("Hands");


    private Player _player;

    public void SetHandsTrigger(Player player)
    {
        _animator.SetTrigger(InHands);
        _player = player;
    }

    private void Update()
    {
        if (_player != null)
        {
            float turningDirection = _player.PlayerAnimator.TurningDirection;
            float desiredRotation = Mathf.Lerp(0, _maxRotation, Mathf.Abs(turningDirection));
            Vector3 rotation = new Vector3(0, 0, desiredRotation * -Mathf.Sign(turningDirection));
            transform.eulerAngles = rotation;
        }
    }
}
