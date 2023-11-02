using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Arrow : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected PaginationScroll _scroll;
    [SerializeField] private Arrow _anotherArrow;
    
    protected int _disablePage;
    private ArrowState _state;
    public event Action<Direction> ArrowPressed;

    protected void Scroll(Direction direction)
    {
        if (_state == ArrowState.Blocked)
            return;
        ArrowPressed?.Invoke(direction);
    }
    public abstract void OnPointerDown(PointerEventData eventData);

    private void OnEnable()
    {
        _scroll.PageChanged += OnPageChanged;
    }
    
    private void OnDestroy()
    {
        _scroll.PageChanged -= OnPageChanged;
    }

    public void SetActive(bool state)
    {
        _state = ArrowState.Active;
        gameObject.SetActive(state);
    }
    
    private void OnPageChanged(int currentPage)
    {
        if (currentPage == _disablePage)
        {
            _state = ArrowState.Disabled;
            _anotherArrow.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            SetActive(true);
        }
    }
}
