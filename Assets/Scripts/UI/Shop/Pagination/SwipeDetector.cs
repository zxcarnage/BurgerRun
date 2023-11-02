using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetector : MonoBehaviour, IDragHandler, IBeginDragHandler,IEndDragHandler
{
    [SerializeField] private float _minVectorXLength;
    private Vector2 _startDragPostion;
    private Direction _swipeDirection;
    private bool _isSwipeStarted;
    private Vector2 _currentDragVector;

    public event Action<Direction> SwipeDetected;

    private void Awake()
    {
        _isSwipeStarted = false;
        _swipeDirection = Direction.None;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isSwipeStarted = true;
        _startDragPostion = eventData.pressPosition;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (_isSwipeStarted)
        {
            _currentDragVector = eventData.position - _startDragPostion;
            if (Mathf.Abs(_currentDragVector.x) >= Mathf.Abs(_minVectorXLength))
            {
                _swipeDirection = _currentDragVector.x - _startDragPostion.x > 0 ? Direction.Left : Direction.Right;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isSwipeStarted && _swipeDirection != Direction.None)
        {
            SwipeDetected?.Invoke(_swipeDirection);
        }
        _isSwipeStarted = false;
    }
}
