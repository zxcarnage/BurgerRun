using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SwipeDetector))]
[RequireComponent(typeof(Scroller))]
public class PaginationScroll : MonoBehaviour
{
    [SerializeField] private int _pagesCount;
    [SerializeField] private Page _page;
    [SerializeField] private RectTransform _pageContainer;
    [SerializeField] private RectTransform _targetSize;
    [SerializeField] private BuyWithYansButton _buyWithYansButton;
    [SerializeField] private Arrow[] _arrows;
    [SerializeField] private Currensy _money;
    [SerializeField] private ShopCanvas _shopCanvas;
    [SerializeField] private Button[] _buttonsToBlock;
    [SerializeField] private BuyButton _mainButton;
    [Tooltip("Additional Page Costs")]
    [SerializeField] private float[] _pageCosts;

    public int MaxPage => _pagesCount;
    public int PreviousPage => _previousPage;

    public event Action<int> PageChanged;

    private int _pageCounter => _scroller.CurrentPage;
    private int _previousPage;
    private List<Page> _pages;
    private Scroller _scroller;
    private SwipeDetector _detector;

    private void Awake()
    {
        _scroller = GetComponent<Scroller>();
        _detector = GetComponent<SwipeDetector>();
        ServiceLocator.Shop = this;
    }

    private void OnEnable()
    {
        foreach (var arrow in _arrows)
        {
            arrow.ArrowPressed += OnArrowPressed;
        }

        _detector.SwipeDetected += OnArrowPressed;
    }

    public float GetCurrentPageCost()
    {
        return _pageCosts[_pageCounter - 1];
    }

    public Element GetActiveElement()
    {
        foreach (var page in _pages)
        {
            var activeElement = page.GetActiveElement();
            if (activeElement)
                return activeElement;
        }

        return null;
    }

    public Element TryGetLockedCommon()
    {
        List<Element> lockedElements = new List<Element>(); 
        foreach (var page in _pages)
        {
            Debug.Log("Page " + page);
            lockedElements.AddRange(page.GetAllLockedCommon());
        }

        if (lockedElements.Count == 0)
            return null;
        return lockedElements[Random.Range(0, lockedElements.Count - 1)];
    }

    public List<Element> TryGetLocked()
    {
        List<Element> lockedElements = new List<Element>(); 
        foreach (var page in _pages)
        {
            lockedElements.AddRange(page.GetLockedElements());
        }

        if (lockedElements.Count == 0)
            return null;
        return lockedElements;
    }
    
    private void OnDisable()
    {
        foreach (var arrow in _arrows)
        {
            arrow.ArrowPressed -= OnArrowPressed;
        }
        _detector.SwipeDetected -= OnArrowPressed;
    }

    public List<Element> TryGetLockedOnPage(int pageCount)
    {
        List<Element> lockedElements = new List<Element>();
        lockedElements = _pages[pageCount - 1].GetLockedElements();
        if (lockedElements.Count == 0)
            return null;
        return lockedElements;
    }

    public bool TryBuy(float cost, Action onLastElementPageUnlocking = null)
    {
        if (_pages[_pageCounter - 1].IsOpened)
            return false;
        bool decreasingResult = _money.TryDecreaseMoney(cost);
        if(decreasingResult) 
            _pages[_pageCounter - 1].Buy(_buttonsToBlock,onLastElementPageUnlocking);
        return decreasingResult;
    }

    public void UnlockAll()
    {
        foreach (var page in _pages)
        {
            page.UnlockAll();
        }
    }

    private void Start()
    {
        InitializePages();
        InitializeScroller();
        InitializeArrows();
        _buyWithYansButton.Init();
        _mainButton.Init();
        _shopCanvas.gameObject.SetActive(false);
    }

    private void InitializeScroller()
    {
        _scroller.Init();
    }

    private void InitializePages()
    {
        _pages = new List<Page>();
        for (int i = 0; i < _pagesCount; i++)
        {
            var page = Instantiate(_page, _pageContainer);
            _pages.Add(page);
            page.Initialize(i);
            page.Resize(_targetSize);
        }
    }

    private void InitializeArrows()
    {
        PageChanged?.Invoke(_pageCounter);
    }

    private void OnArrowPressed(Direction direction)
    {
        _previousPage = _pageCounter;
        _scroller.Scroll(direction, _targetSize.rect.size, _pagesCount);
        PageChanged?.Invoke(_pageCounter);
    }
}
