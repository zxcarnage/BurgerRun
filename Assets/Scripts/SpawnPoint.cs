using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Player _player;

    private Player _playerSpawned;
    
    public Player Player => _playerSpawned;

    private void Awake()
    {
        ServiceLocator.SetSpawnPoint(this);
        _playerSpawned = Instantiate(_player, transform.position, transform.rotation);
        _playerSpawned.transform.SetParent(transform);
    }
}
