using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerJumpMover : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 5.0f;
    [SerializeField] private float timeToReachTarget = 1.0f;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float _leftEdge;
    [SerializeField] private float _rightEdge;
    [SerializeField] private float _horizontalSpeed;
    
    private Vector3 _xMovementVector = Vector3.zero;
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private PlayerAnimator _animator;

    private void Awake()
    {
        _animator = GetComponent<PlayerAnimator>();
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ServiceLocator.Player.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        ServiceLocator.Player.Died -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        StopCoroutine(JumpToTargetRoutine(transform,  false));
        enabled = false;
    }

    public void JumpToTarget(Transform target,  bool isOnPad)
    {
        StartCoroutine(JumpToTargetRoutine(target,  isOnPad));
    }
    
    
    private IEnumerator JumpToTargetRoutine(Transform target, bool isOnPad)
    {
        ServiceLocator.Player.BlockMovement();
        Vector3 toTarget = new Vector3(0f,0f,target.position.z - _rigidbody.position.z);
        Vector3 initialPosition = _rigidbody.position;
        float horizontalDistance = new Vector3(0, 0, toTarget.z).magnitude;

        float startTime = Time.time;
        if (!isOnPad)
            _animator.StartJumpAnimation();
        _playerInput.ResetDrag();
    
        float jumpEndTime = startTime + timeToReachTarget; // Время окончания прыжка

        while (Time.time < jumpEndTime)
        {
            float elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / timeToReachTarget;

            float verticalDistance = jumpCurve.Evaluate(normalizedTime) * jumpHeight;
            float horizontalDistanceAtTime = normalizedTime * horizontalDistance;
            _xMovementVector = new Vector3(_playerInput.GetXDirection(), 0f, 0f);
            Vector3 newPosition = initialPosition + toTarget.normalized * horizontalDistanceAtTime;
            newPosition.y = initialPosition.y + verticalDistance;
            newPosition.x = initialPosition.x + _xMovementVector.x * _horizontalSpeed * Time.fixedDeltaTime;
            newPosition.x = Mathf.Clamp(newPosition.x, _leftEdge, _rightEdge);

            _rigidbody.MovePosition(newPosition);

            yield return null;
        }

        if (isOnPad)
        {
            // Здесь не устанавливаем позицию в target.position
            _animator.JumpEnded();
            ServiceLocator.Player.UnlockMovement();
        }
    }



}
