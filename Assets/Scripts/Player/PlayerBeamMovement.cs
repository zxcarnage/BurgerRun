using DG.Tweening;
using UnityEngine;

public class PlayerBeamMovement : MonoBehaviour
{
    [SerializeField] private BeamUI _beamUIPrefab;
    private BeamUI _beamUI;

    [SerializeField] private float _speedOfFalling = 5;
    [SerializeField] private float _speedOfResisting = 8;
    private int _fallingDirection = 1;
    [SerializeField] private float _maxAngle = 60f;

    [SerializeField] private CapsuleCollider _playerCollider;

    private bool _insideBeam, _playerLost;

    private PlayerInput _playerInput;
    private PlayerSkinSwitcher _switcher;
    private Player _player;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _switcher = GetComponent<PlayerSkinSwitcher>();
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
        _playerCollider = newRig.GetComponent<CapsuleCollider>();
    }

    public void StartBeamMovement()
    {
        _beamUI = Instantiate(_beamUIPrefab, InGameCanvas.Instance.Canvas.transform);
        _insideBeam = true;
    }

    private void Update()
    {
        if (_insideBeam)
        {
            float playerInput = _playerInput.GetXDirection();
            transform.Rotate(0, 0, _fallingDirection * _speedOfFalling * Time.deltaTime + (-playerInput * _speedOfResisting));

            if (_fallingDirection == 1 && transform.localRotation.z < 0)
                _fallingDirection = -1;
            else if (_fallingDirection == -1 && transform.localRotation.z > 0)
                _fallingDirection = 1;

            float value = -AngleConverter.WrapAngle(transform.rotation.eulerAngles.z) / _maxAngle;
            _beamUI.ChangeSliderValue(value);
        }
    }

    private void LateUpdate()
    {
        if (_insideBeam && !_playerLost && Mathf.Abs(AngleConverter.WrapAngle(transform.localRotation.eulerAngles.z)) > _maxAngle)
        {
            PlayerLost();
        }
    }

    private void PlayerLost()
    {
        _playerLost = true;
        _insideBeam = false;
        _playerCollider.enabled = false;
        _player = GetComponent<Player>();
        _player.PlayerMover.Block(true);
        Vector3 fallingDirectionVector = Vector3.down + (Vector3.left * _fallingDirection * 0.2f);
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, AngleConverter.UnwrapAngle(150 * _fallingDirection));
        _player.PlayerAnimator.PlayFallAnimation(_fallingDirection);
        Camera.Instance.StopFollowing();
        DOTween.Sequence().Append(transform.DORotate(newRotation, 0.5f))
                          .Append(_player.PlayerMover.MoveTo(transform.position + fallingDirectionVector * 4, 0.5f))
                          .AppendCallback(() => Destroy(_beamUI.gameObject))
                          .AppendCallback(ServiceLocator.GetEndCanvas().ShowLosePanel);
    }

    public void EndBeamMovement()
    {
        _insideBeam = false;
        Destroy(_beamUI.gameObject);
    }

    public void BalanceItOut()
    {
        _insideBeam = false;
        transform.DORotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0), 0.5f);
        _beamUI.BalanceSlider.DOValue(0, 0.5f);
    }
}
