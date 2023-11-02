using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class TightRopePlayerMover : MonoBehaviour
{
    [SerializeField] private float _pathTime;
    [SerializeField] private Animator _animator;
    [Space(5)][Header("Angular Movement")]
    [SerializeField] private float _leftAngle;
    [SerializeField] private float _rightAngle;

    private Vector3 _destination;
    private TightRope _tightRope;
    private Quaternion _leftQuaternion;
    private Quaternion _rightQuaternion;
    private PlayerMover _playerMover;
    private bool _onWay = true;
    private Tween _moveTween;

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
        _leftQuaternion = Quaternion.Euler(new Vector3(0,0,_leftAngle));
        _rightQuaternion = Quaternion.Euler(new Vector3(0,0,_rightAngle));
    }

    public void GiveTightRope(TightRope tightRope)
    {
        _tightRope = tightRope;
    }

    public void GiveDestination(Transform destination)
    {
        _destination = destination.position;
    }

    private void OnEnable()
    {
        _moveTween = transform.DOMove(_destination, _pathTime).SetEase(Ease.Linear).OnComplete(DestinationReached);
        _animator.SetTrigger("BeamWalk");
        _tightRope.PlayerFalls += OnPlayerFalls;
    }

    private void OnDisable()
    {
        _tightRope.PlayerFalls -= OnPlayerFalls;
    }

    private void DestinationReached()
    {
        _onWay = false;
        transform.rotation = Quaternion.identity;
        _playerMover.Block(false);
        _animator.SetTrigger("Idle");
        _tightRope.enabled = false;
        enabled = false;
    }
    
    private void Update()
    {
        if(_onWay)
            Rotate();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Lerp(_rightQuaternion, _leftQuaternion, (_tightRope.Value + 1)/2);
    }

    private void OnPlayerFalls(float value)
    {
        GetComponent<Player>().Fall(value);
        _moveTween.Kill();
        _tightRope.enabled = false;
        enabled = false;
    }
}
