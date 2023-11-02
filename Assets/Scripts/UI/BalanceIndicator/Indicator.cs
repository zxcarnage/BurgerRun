using UnityEngine;

public class Indicator: MonoBehaviour
{
    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;
    [SerializeField] private RectTransform _arrow;
    [SerializeField] private TightRope _tightRope;

    private Quaternion _minQuat;
    private Quaternion _maxQuat;
    
    private void Start()
    {
        _minQuat = Quaternion.Euler(Vector3.forward * _minAngle);   
        _maxQuat = Quaternion.Euler(Vector3.forward * _maxAngle);   
    }

    private void Update()
    {
        _arrow.rotation = Quaternion.Lerp(_maxQuat, _minQuat, (_tightRope.Value + 1)/2);
    }
}
