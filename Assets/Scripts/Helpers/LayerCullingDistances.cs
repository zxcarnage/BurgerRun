using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Camera))]
public class LayerCullingDistances : MonoBehaviour
{
    [SerializeField]
    private float[] _cullingDistances = new float[32];

    private void Awake()
    {
        SetCullingDistances();
    }

    private void OnValidate()
    {
        SetCullingDistances();
    }

    private void SetCullingDistances()
    {
        GetComponent<UnityEngine.Camera>().layerCullDistances = _cullingDistances;
    }
}