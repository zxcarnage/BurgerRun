using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCanvas : MonoBehaviour
{
    [SerializeField] private float _distanceToCamera;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;

    private void Update()
    {
        _canvas.transform.eulerAngles = _rotation;
        Vector3 position = transform.position;
        position.z = _camera.transform.position.z + _distanceToCamera;
        _canvas.transform.position =  position;
    }
}
