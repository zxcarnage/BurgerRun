using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour, IObstacle
{
    [SerializeField] private ParticleSystem _splashPrefab;

    private ParticleSystem _particles;

    public void AffectPlayer(Player player)
    {
        _particles = Instantiate(_splashPrefab.gameObject, player.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        _particles.Play();
        player.DisableCollider();
    }
}
