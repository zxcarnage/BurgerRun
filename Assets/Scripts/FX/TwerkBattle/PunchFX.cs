using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PunchFX : MonoBehaviour
{
    private ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    public void Punch()
    {
        _particle.Play();
    }
}
