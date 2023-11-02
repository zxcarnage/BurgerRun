using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRollingMovement : MonoBehaviour
{
    [SerializeField] private float _newColliderHeight = 0.5f;
    [SerializeField] private float _newColliderYCenter = 0.3f;
    private float _oldColliderHeight;
    private float _oldColliderYCenter;

    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private Transform _transformToRotate;
    [SerializeField] private Vector3 _rotationOfModel;

    [SerializeField] private float _speedOfMovement;

    private PlayerInput _playerInput;

    private bool _insideBeams;

    private Vector3 _forwardVector;
    private PlayerSkinSwitcher _switcher;

    private ParallelBeams _beams;
    [SerializeField] private ParallelLanes _currentLane;

    private float _timer;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _switcher = GetComponent<PlayerSkinSwitcher>();
        _oldColliderHeight = _capsuleCollider.height;
        _oldColliderYCenter = _capsuleCollider.center.y;
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
        _capsuleCollider = newRig.GetComponent<CapsuleCollider>();
        _transformToRotate = newRig.GetComponent<Transform>();
    }


    public void SetUpCollider(bool toNewValues = true)
    {
        _capsuleCollider.height = toNewValues ? _newColliderHeight : _oldColliderHeight;
        _capsuleCollider.center = new Vector3(_capsuleCollider.center.x, 
                                              toNewValues ? _newColliderYCenter : _oldColliderYCenter,
                                              _capsuleCollider.center.z);
    }

    public void SetRotationForMovement(float value)
    {
        transform.DORotate(new Vector3(value, transform.eulerAngles.y, transform.eulerAngles.z), 1f)
            .OnComplete(() => _forwardVector = transform.forward);
    }

    public void SetBeams(ParallelBeams beams)
    {
        _beams = beams;
    }

    public void StartMovement(ParallelLanes lane)
    {
        _insideBeams = true;
        _currentLane = lane;
    }

    private void Update()
    {
        if (_insideBeams)
        {
            transform.Rotate(_rotationOfModel * Time.deltaTime);
            transform.position += _forwardVector * _speedOfMovement * Time.deltaTime;

            if (Mathf.Abs(_playerInput.GetXDirection()) >= 0.5f)
            {
                int currentLaneIndex = (int)_currentLane;
                int directionToMove = Mathf.Sign(_playerInput.GetXDirection()) > 0 ? 1 : -1;
                int newLaneIndex = Mathf.Clamp(currentLaneIndex + directionToMove, 0, 2);
                _currentLane = (ParallelLanes)newLaneIndex;
                transform.DOMoveX(_beams.GetLaneXPosition(newLaneIndex), 0.2f);
                _playerInput.Reset();
            }
        }
    }

    private void SwitchLanes()
    {

    }

    public void StopMovement()
    {
        SetUpCollider(false);
        _insideBeams = false;
    }
}

public enum ParallelLanes
{
    Left,
    Center,
    Right
}
