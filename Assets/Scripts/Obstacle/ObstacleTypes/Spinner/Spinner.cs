using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation = new Vector3(0, 60, 0);

    private void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }
}
