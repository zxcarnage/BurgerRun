using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private MeshRenderer _conveyorRenderer;
    [SerializeField] private float _speedOfConveyor = 2f;
    [SerializeField] private float _speedOfObjects = 4f;
    [SerializeField] private float _distanceInBetweenObjects = 0.5f;

    [SerializeField] private List<Transform> _pathPoints;

    private List<Food> _foodsOnConveyor = new List<Food>();

    private BoxCollider _boxCollider;
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        _conveyorRenderer.material.mainTextureOffset += new Vector2(0, -_speedOfConveyor) * Time.deltaTime;
    }

    private void Start()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, _boxCollider.size * 0.5f, Quaternion.identity);
        float delayBetweenObjects = 0;
        foreach (var collider in colliders)
        {
            
            if (collider.TryGetComponent(out Food food))
            {
                _foodsOnConveyor.Add(food);
            }
        }

        if (_foodsOnConveyor.Count > 0)
        {
            Vector3[] path = _pathPoints.Select(n => n.position).ToArray();
            Debug.Log(path[0]);
            Debug.Log(path[1]);
            Debug.Log(path[2]);
            Debug.Log(path[3]);
            for (int i = 0; i < _foodsOnConveyor.Count; i ++)
            {
                Food food = _foodsOnConveyor[i];
                food.gameObject.transform.position = _pathPoints[3].position;
                food.transform.DOPath(path, _speedOfObjects).SetLoops(-1).SetDelay(delayBetweenObjects).SetEase(Ease.Linear);
                delayBetweenObjects += _distanceInBetweenObjects;
            }
        }
        
    }

}
