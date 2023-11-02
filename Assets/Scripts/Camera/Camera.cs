using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(CameraFollower))]
public class Camera : MonoBehaviour
{
    public static Camera Instance;

    [SerializeField] private Vector3 _offset;
    public Vector3 Offset => _offset;
    private Vector3 _startingOffset;

    [SerializeField] private Vector3 _rotation;
    private Vector3 _startingRotation;

    [SerializeField] private LevelSpawner _spawner;
    private SpawnPoint _playerSpawnPoint;
    private CameraFollower _cameraFollower;

    private bool _canFollow = true;

    private Transform _targetToFollow;

    private void Awake()
    {
        _cameraFollower = GetComponent<CameraFollower>();
        Instance = this;
        _startingOffset = _offset;
        _startingRotation = _rotation;
    }

    private void OnEnable()
    {
        _spawner.LevelSpawned += OnLevelSpawned;
    }

    private void OnDisable()
    {
        _spawner.LevelSpawned -= OnLevelSpawned;
    }

    private void Start()
    {
        FindSpawnPoint();
    }

    private void OnLevelSpawned()
    {
        FindSpawnPoint();
        ResetCamera();
        _canFollow = true;
    }

    private void ResetCamera()
    {
        _offset = _startingOffset;
        _rotation = _startingRotation;
    }

    private void FindSpawnPoint()
    {
        _playerSpawnPoint = FindObjectOfType<SpawnPoint>();
        _targetToFollow = _playerSpawnPoint.Player.transform;
    }

    private void LateUpdate()
    {
        if (_canFollow) 
            _cameraFollower.FollowPlayer(_targetToFollow.position, _offset, _rotation);
    }

    public void StopFollowing()
    {
        _canFollow = false;
    }

    public void UpdateZOffset(float distance)
    {
        _offset = _startingOffset - new Vector3(0, 0, distance);
    }

    public void UpdateOffsetBy(Vector3 offset, float duration)
    {
        //_offset = _startingOffset + offset;
        DOTween.To(() => _offset, x => _offset = x, _startingOffset + offset, duration);
    }

    public void UpdateRotationBy(Vector3 newRotation, float duration)
    {
        //_rotation = _startingRotation + newRotation;
        DOTween.To(() => _rotation, x => _rotation = x, _startingRotation + newRotation, duration);
    }

    public void ChangeTargetToFollow(Transform newTarget, float durationToChange = 2f)
    {
        _targetToFollow = newTarget;
        //DOTween.To(() => _targetToFollow.position, x => _targetToFollow.position = x, newTarget.position, durationToChange);
    }
    
}
