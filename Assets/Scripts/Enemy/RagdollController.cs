using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody[] _allRigidBodies;
    [SerializeField] private List<CharacterJoint> characterJoints = new (16);

    private bool _isPhysical;

    private void Awake()
    {
        for (int i = 0; i < _allRigidBodies.Length; i++)
        {
            _allRigidBodies[i].isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (_isPhysical)
        {
            for (int i = 0; i < _allRigidBodies.Length; i++)
            {
                _allRigidBodies[i].velocity = new Vector3(0, _allRigidBodies[i].velocity.y, _allRigidBodies[i].velocity.z);
            }
        }
    }

    public void StopAllRigidbodies()
    {
        foreach (var rb in _allRigidBodies)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    public void MakePhysical()
    {
        _animator.enabled = false;
        for (int i = 0; i < _allRigidBodies.Length; i++)
        {
            _allRigidBodies[i].isKinematic = false;
        }
        _isPhysical = true;
    }
}
