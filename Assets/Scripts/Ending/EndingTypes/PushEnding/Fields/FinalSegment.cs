using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FinalSegment : MonoBehaviour
{
    [SerializeField] private Color _finalColor;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void AffectRagdoll(Rigidbody rigidBodyToStop, RagdollController ragdollController)
    {
        _renderer.material.color = _finalColor;
        rigidBodyToStop.velocity = new Vector3(0f, rigidBodyToStop.velocity.y, 0f);
        ragdollController.StopAllRigidbodies();
    }

    public void Positionate(Segment previousSegment)
    {
        var prefabZHalfSize = previousSegment.GetComponent<Renderer>().bounds.extents.z;
        var newZPosition = previousSegment.transform.position + new Vector3(0f, 0f, prefabZHalfSize);
        transform.position = newZPosition;
    }
}
