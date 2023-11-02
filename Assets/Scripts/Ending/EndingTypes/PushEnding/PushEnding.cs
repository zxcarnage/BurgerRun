using UnityEngine;

public class PushEnding : MonoBehaviour, IEnding
{
    [SerializeField] private PushFieldSpawner _pushFieldSpawner;

    public void Spawn(Vector3 original)
    {
        _pushFieldSpawner.Spawn();
    }
}
