using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    private Vector2 _speedOfWater = new Vector2(0.1f,0);
    private Renderer _renderer;
    private void Start()
    {
        if (TryGetComponent(out Renderer renderer))
        {
            _renderer = renderer;   
        }
    }

    private void Update()
    {
        if (_renderer != null) 
        {
            _renderer.material.mainTextureOffset += _speedOfWater * Time.deltaTime;
        }
    }
}
