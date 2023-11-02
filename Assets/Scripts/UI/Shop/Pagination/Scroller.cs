using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Scroller : MonoBehaviour
{
    [SerializeField] private RectTransform _content;

    [Space(10)] [Header("Animation Info")] [SerializeField]
    private float _animationDuration;
    
    private int _currentPage = 1;
    private int _previousPage;
    private Arrow _arrow;
    public int CurrentPage => _currentPage;
    public int PreviousPage => _previousPage;
    private Vector2 _targetScrollPosition;
    private Sequence _scrollSequence;
    
    public void Init()
    {
        _scrollSequence = DOTween.Sequence().SetAutoKill(false);
    }

    public void Scroll(Direction direction, Vector2 scrollAmount, int maxPages)
    {
        switch (direction)
        {
            case Direction.Left:
                TryScrollLeft(scrollAmount);
                break;
            case Direction.Right:
                TryScrollRight(scrollAmount, maxPages);
                break;
        }
    }

    private void TryScrollRight(Vector2 scrollAmount, int maxPages)
    {
        if(_currentPage >= maxPages)
            return;
        ScrollRight(scrollAmount);
    }

    private void TryScrollLeft(Vector2 scrollAmount)
    {
        if(_currentPage - 1 <= 0)
            return;
        ScrollLeft(scrollAmount);
    }
    
    private void ScrollLeft(Vector2 scrollAmount)
    {
        _previousPage = _currentPage;
        _currentPage--;
        _targetScrollPosition = new Vector2(_targetScrollPosition.x + scrollAmount.x,
            _content.anchoredPosition.y);
        ScrollAnimation(_targetScrollPosition);
    }

    private void ScrollRight(Vector2 scrollAmount)
    {
        _previousPage = _currentPage;
        _currentPage++;
        _targetScrollPosition = new Vector2(_targetScrollPosition.x - scrollAmount.x,
            _content.anchoredPosition.y);
        ScrollAnimation(_targetScrollPosition);
    }

    private void ScrollAnimation(Vector2 targetPosition)
    {
        _scrollSequence.Append(_content.DOAnchorPosX(targetPosition.x,_animationDuration)).SetEase(Ease.Linear);
    }
}
