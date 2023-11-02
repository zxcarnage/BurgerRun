using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootRagdoll : MonoBehaviour
{
    [SerializeField] private Rigidbody _parentRigidbody;

    [SerializeField]
    private RagdollController _ragdollController;
    public Action<Segment> PassedSegment;

    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FinalSegment finalSegment))
            finalSegment.AffectRagdoll(_parentRigidbody, _ragdollController);
        else if (other.gameObject.TryGetComponent(out Segment segment))
            PassedSegment?.Invoke(segment);
    }
}