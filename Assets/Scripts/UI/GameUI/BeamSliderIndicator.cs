using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamSliderIndicator : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private RectTransform _middleIndicatorPosition;
    [SerializeField] private RectTransform _rightIndicatorPosition;
    [SerializeField] private RectTransform _leftIndicatorPosition;

    private void Start()
    {
        _slider.onValueChanged.AddListener(ChangeIndicatorPosition);
    }

    private void LateUpdate()
    {
        ChangeIndicatorPosition(_slider.value);
    }

    public void ChangeIndicatorPosition(float value)
    {
        if (value == 0)
        {
            transform.position = _middleIndicatorPosition.position;
            transform.localRotation = _middleIndicatorPosition.localRotation;
        }
        if (value < 0)
        {
            float xPosition = Mathf.Lerp(_middleIndicatorPosition.position.x, _leftIndicatorPosition.position.x, value / -1);
            float yPosition = Mathf.Lerp(_middleIndicatorPosition.position.y, _leftIndicatorPosition.position.y, value / -1);
            float zPosition = Mathf.Lerp(_middleIndicatorPosition.position.z, _leftIndicatorPosition.position.z, value / -1);

            float xRotation = Mathf.Lerp(_middleIndicatorPosition.localRotation.x, _leftIndicatorPosition.localRotation.x, value / -1);
            float yRotation = Mathf.Lerp(_middleIndicatorPosition.localRotation.y, _leftIndicatorPosition.localRotation.y, value / -1);
            float zRotation = Mathf.Lerp(_middleIndicatorPosition.localRotation.z, _leftIndicatorPosition.localRotation.z, value / -1);

            transform.position = new Vector3(xPosition, yPosition, zPosition);
            transform.localRotation = new Quaternion(xRotation, yRotation, zRotation, 1);
        }
        else if (value > 0)
        {
            float xPosition = Mathf.Lerp(_middleIndicatorPosition.position.x, _rightIndicatorPosition.position.x, value  / 1f);
            float yPosition = Mathf.Lerp(_middleIndicatorPosition.position.y, _rightIndicatorPosition.position.y, value / 1f);
            float zPosition = Mathf.Lerp(_middleIndicatorPosition.position.z, _rightIndicatorPosition.position.z, value / 1f);

            float xRotation = Mathf.Lerp(_middleIndicatorPosition.localRotation.x, _rightIndicatorPosition.localRotation.x, value / 1f);
            float yRotation = Mathf.Lerp(_middleIndicatorPosition.localRotation.y, _rightIndicatorPosition.localRotation.y, value / 1f);
            float zRotation = Mathf.Lerp(_middleIndicatorPosition.localRotation.z, _rightIndicatorPosition.localRotation.z, value / 1);

            transform.position = new Vector3(xPosition, yPosition, zPosition);
            transform.localRotation = new Quaternion(xRotation, yRotation, zRotation, 1);
        }

    }
}
