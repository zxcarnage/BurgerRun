using System;
using UnityEngine;

public class PagePoints : MonoBehaviour
{
    [SerializeField] private PaginationScroll _paginationScroll;
    [SerializeField] private PagePoint[] _points;

    private int _previousPage => _paginationScroll.PreviousPage;

    private void OnEnable()
    {
        _paginationScroll.PageChanged += OnPageChanged;
    }

    private void OnDisable()
    {
        _paginationScroll.PageChanged -= OnPageChanged;
    }

    private void OnPageChanged(int currentPage)
    {
        _points[currentPage - 1].Activate();
        if (_previousPage == 0)
            return;
        _points[_previousPage - 1].Deactivate();
    }
}
