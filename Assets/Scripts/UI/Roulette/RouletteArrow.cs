using System;
using UnityEngine;

public class RouletteArrow : MonoBehaviour
{
    [SerializeField] private float _minAgnle;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _movingTime;

    public event Action<RouletteSegment> SegmentCrossed;
    public event Action<RouletteSegment> ArrowStopped;

    private Quaternion _minQuaternion;
    private Quaternion _maxQuaternion;
    private RouletteSegment _segment;
    private Quaternion _targetQuaternion;
    private bool _isGoingToMax = true;
    private bool _arrowStopped = false;
    private void Start()
    {
        _minQuaternion = Quaternion.Euler(new Vector3(transform.rotation.x,transform.rotation.y ,_minAgnle));
        _maxQuaternion = Quaternion.Euler(new Vector3(transform.rotation.x,transform.rotation.y , _maxAngle));
        _targetQuaternion = _maxQuaternion;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            MoveArrow();
        else
        {
            _arrowStopped = true;
            transform.rotation = transform.rotation;
            ArrowStopped?.Invoke(_segment);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out RouletteSegment segment))
        {
            _segment = segment;
            SegmentCrossed?.Invoke(segment);
        }
    }

    private void MoveArrow()
    {
        if (_arrowStopped)
            return;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetQuaternion, _movingTime * Time.deltaTime);
        TryRotateArrow();
    }

    private void TryRotateArrow()
    {
        if (_arrowStopped == false && Quaternion.Angle(transform.rotation ,_targetQuaternion) < 0.1f)
        {
            if (_isGoingToMax)
            {
                _targetQuaternion = _minQuaternion;
            }
            else
            {
                _targetQuaternion = _maxQuaternion;
            }
            _isGoingToMax = !_isGoingToMax;
        }
    }

    private void OnEnable()
    {
        _arrowStopped = false;
    }
}
