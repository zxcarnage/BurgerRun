using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Segment : MonoBehaviour
{
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private GameObject _lockWall;

    [SerializeField] private Color _color1;
    [SerializeField] private Color _color2;

    [SerializeField] private int _maximumSegmentNumber = 95;
    
    protected Renderer _renderer;


    private int _segmentNumber;
    private GameObject _wall;
    private float _multiplyier;

    public float Multiplyier => _multiplyier;
    public GameObject Wall => _wall;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Highlight()
    {
        Color newColor = Color.Lerp(_color1, _color2, (float)_segmentNumber / (float)_maximumSegmentNumber);
        
        _renderer.material.color = newColor;
    }

    public void SetMultiplyier(int segmentNumber)
    {
        _segmentNumber = segmentNumber;

        _multiplyier = segmentNumber * 0.2f;
        var result = 1.0f + _multiplyier;
        if (result > _playerStats.MaxDistance)
        {
            var wallPosition = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + GetComponent<Renderer>().bounds.size.z);
            _wall = Instantiate(_lockWall, wallPosition, Quaternion.identity, transform);
            if (result > 20)
            {
                _wall.GetComponent<LockBrickWall>().DisableVisual();
                _renderer.enabled = false;
                _text.enabled = false;
            }
        }
        _text.text = result.ToString("0.0") + "x";
    }
    public void TryDisableWall()
    {
        if(_wall)
            _wall.SetActive(false);
    }
}
