using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class FinalKickEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Animator _animator;

    private RagdollController _ragdollController;
    [SerializeField] private RootRagdoll _rootRagdoll;
    [SerializeField] private Vector3 _newCameraOffsetIncrement;
    [SerializeField] private Vector3 _newCameraRotationIncrement;
    [SerializeField] private float _timeToChangeCamera = 2f;
    [SerializeField] private Transform _newTargetForCamera;
    [SerializeField] private AudioClip _pushAudio;

    [SerializeField] private float _minYAngle = 0.05f;
    [SerializeField] private float _maxYAngle = 0.5f;
    [SerializeField] private float _maxPush = 135;

    private float _playerFat;
    private float _playerPushPower;
    private Rigidbody _rigidbody;
    private BoxCollider _collider;
    private LevelEndCanvas _levelEnd;
    private float _multiplyier = 1f;
    private Vector3 _pushVector;
    private Segment _previousSegment;

    private float _timer = 0;
    private float _timeToForceEnd = 15f;
    private bool _pushed;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _levelEnd = ServiceLocator.GetEndCanvas();
        _ragdollController = GetComponent<RagdollController>();
        _animator.SetTrigger("Twerk");
        _rootRagdoll.PassedSegment += HandleSegmentTrigger;
    }

    private void OnEnable()
    {
        _timer = 0;
        _pushed = false;
    }

    private void Update()
    {
        if (_pushed == false) return;

        _timer += Time.deltaTime;
        if (_timer >=  _timeToForceEnd)
        {
            _timer = 0;
            StartCoroutine(WinRoutine());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if((other.gameObject.TryGetComponent(out Segment segment) || other.gameObject.TryGetComponent(out FinalSegment finalSegment)) && enabled)
            if (Mathf.Abs(_rigidbody.velocity.z) <= 1f)
            {
                StartCoroutine(WinRoutine());
            }
    }

    private IEnumerator WinRoutine()
    {
        enabled = false;
        yield return new WaitForSeconds(1f);
        if (_levelEnd.CanShowSecretGiftPanel)
            _levelEnd.ShowSecretGiftPanel(_multiplyier);
        else
            _levelEnd.ShowWinPanel(_multiplyier);
    }

    private void HandleSegmentTrigger(Segment segment)
    {
        if(!_previousSegment)
            _previousSegment = segment;
        segment.Highlight();
        _previousSegment = segment;
        _multiplyier += 0.2f;
    }

    public void InterractWith(Player player)
    {
        _ragdollController.MakePhysical();
        _playerFat = player.Health > 10 ? player.Health : 10;
        player.PlayerAnimator.PlayerOnGround();
        _playerPushPower = _playerStats.PushStrength;
        //_camera.Priority = 100;
        Camera.Instance.ChangeTargetToFollow(_newTargetForCamera.transform);
        Camera.Instance.UpdateOffsetBy(_newCameraOffsetIncrement, _timeToChangeCamera);
        Camera.Instance.UpdateRotationBy(_newCameraRotationIncrement, _timeToChangeCamera);
        _collider.enabled = false;
        Push();
    }

    private void Push()
    {
        var transformPosition = transform.position;
        var pushPower = _playerPushPower * _playerFat;
        Debug.Log("PushPower" + pushPower);
        if (pushPower < 10000) pushPower = 10000;
        var heightClamped = Mathf.Clamp(pushPower + transformPosition.normalized.y, 0.3f, 0.5f);
        var _pushVector = new Vector3(0,Mathf.Lerp(_maxYAngle, _minYAngle,pushPower / _maxPush),
            1).normalized;
        _rigidbody.AddForce(_pushVector * pushPower,ForceMode.Impulse);
        SoundManager.Instance.PlaySound(_pushAudio);
        _pushed = true;
    }
}
