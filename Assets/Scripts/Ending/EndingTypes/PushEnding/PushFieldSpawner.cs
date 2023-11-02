using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PushFieldSpawner : MonoBehaviour
{
    [SerializeField] private Segment _fieldSegment;
    [SerializeField] private FinalSegment _finalSegment;
    [SerializeField] private GameObject _startSegment;
    [SerializeField] private int _segmentsCount = 100;
    [SerializeField] private PlayerStats _playerStats;
    
    private float _prefabZSize;
    private Vector3 _startSpawnPoint;
    private List<Segment> _segments;

    private void Awake()
    {
        _segments = new List<Segment>();
    }

    private void OnDisable()
    {
        _playerStats.DistanceChanged -= OnDistanceChanged;
    }

    
    private void OnEnable()
    {
        CalculateStartSpawnPoint();
        _playerStats.DistanceChanged += OnDistanceChanged;
    }

    private void CalculateStartSpawnPoint()
    {
        var zSpawnPoint = _startSegment.GetComponent<Renderer>().bounds.size.z;
        _startSpawnPoint = new Vector3(_startSegment.transform.position.x, _startSegment.transform.position.y,
            zSpawnPoint + _startSegment.transform.position.z);
    }

    public void Spawn()
    {
        SpawnField();
    }

    private void SpawnField()
    {
        for (int i = 0; i < _segmentsCount - 2; i++)
        {
            SpawnSegment(_fieldSegment, i);
        }
        
        SpawnFinalSegment();
    }

    private void SpawnSegment(Segment fieldSegment, int segmentNum)
    {
        var segment = Instantiate(fieldSegment, _startSpawnPoint,Quaternion.identity);
        _segments.Add(segment);
        segment.SetMultiplyier(segmentNum);
        PlacePrefab(segment,segmentNum, _startSpawnPoint);
        segment.transform.SetParent(transform);
    }

    private void SpawnFinalSegment()
    {
        var finalSegment = Instantiate(_finalSegment, _startSpawnPoint,Quaternion.identity);
        finalSegment.transform.SetParent(transform);
        finalSegment.Positionate(_segments.Last());
    }

    private void OnDistanceChanged()
    {
        IEnumerable<Segment> activeWallSegments = new List<Segment>();
        activeWallSegments = _segments.Where(x => x.Multiplyier + 1.0f <= _playerStats.MaxDistance && x.Wall);
        foreach (var element in activeWallSegments)
        {
            element.TryDisableWall();
        }
    }

    private void PlacePrefab(Segment segment, int iteration, Vector3 startPosition)
    {
        _prefabZSize = segment.GetComponent<Renderer>().bounds.size.z;
        float newZPosition = iteration * _prefabZSize;
        segment.transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z + newZPosition);
    }
}
