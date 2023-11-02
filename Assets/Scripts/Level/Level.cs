using UnityEngine;

[RequireComponent(typeof(ILevel))]
public class Level : MonoBehaviour
{
    [SerializeField] private Transform _endindPosition;
    
    private ILevel _levelType;

    private void Awake()
    {
        _levelType = GetComponent<ILevel>();
    }

    private void Start()
    {
        _levelType.SpawnEnding(_endindPosition.position);
    }
}
