using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Page : MonoBehaviour
{
    [SerializeField] private int _elementsCount;
    [SerializeField] private Element _element;
    [SerializeField] private ShopSkins _skins;

    [Space(20f)] [Header("UnlockAnimation")] 
    [SerializeField] private float _highlightTime;
    [SerializeField] private int _highlightedElements;

    public bool IsOpened => !_elements.Find(x => x.ElementState == State.Locked);

    private List<Element> _elements;
    private Element _previousElement;
    public void Initialize(int pageNum)
    {
        string elementStatesString = "";
        _elements = new List<Element>();
        for (int i = 0; i < _elementsCount; i++)
        {
            var element = Instantiate(_element, transform);
            element.ElementChosen += OnElementChosen;
            var pageSkinInfo = _skins.SkinPages.PagesList[pageNum].Skins[i];
            element.Initialize(pageSkinInfo);
            _elements.Add(element);
            elementStatesString += " " + element.ElementState;
        }
    }

    private void OnDestroy()
    {
        foreach (var element in _elements)
        {
            element.ElementChosen -= OnElementChosen;
        }
    }

    public void Buy(Button[] buttonsToBlock, Action onLastElementUnlocking = null)
    {
        StartCoroutine(BuyRoutine(buttonsToBlock,onLastElementUnlocking));
    }

    private void OnElementChosen(Element chosenElement)
    {
        _previousElement = ServiceLocator.Shop.GetActiveElement();
        _previousElement.UnChoose();
        chosenElement.Choose();
    }

    public Element GetActiveElement()
    {
        return GetElement(State.Active);
    }

    public List<Element> GetAllLockedCommon()
    {
        return _elements.FindAll(x => x.ElementState == State.Locked && x.Skin.SkinRig == false);
    }

    public void UnlockAll()
    {
        List<Element> _lockedElement = _elements.FindAll(x => x.ElementState == State.Locked);
        foreach (var lockedElement in _lockedElement)
        {
            lockedElement.Unlock();
        }
    }

    private Element GetElement(State elementState)
    {
        Debug.Log("Get Element + " + this);
        return _elements.FirstOrDefault(x => x.ElementState == elementState);
    }

    public List<Element> GetLockedElements()
    {
        return _elements.FindAll(x => x.ElementState == State.Locked);
    }

    private IEnumerator BuyRoutine(Button[] buttons, Action onLastElementUnlocking = null)
    {
        Debug.Log(_elements[0].ElementState + " " + _elements.Count);
        List<Element> lockedElements = _elements.FindAll(x => x.ElementState == State.Locked);
        ChangeButtonState(buttons, false);
        int currentHighlightedElements = 0;
        int chosenElement = 0;
        int previousElementNum = chosenElement;
        while (currentHighlightedElements != _highlightedElements)
        {
            lockedElements[previousElementNum].UnHighlight();
            chosenElement = Random.Range(0, lockedElements.Count - 1);
            lockedElements[chosenElement].Highlight();
            previousElementNum = chosenElement;
            currentHighlightedElements++;
            yield return new WaitForSeconds(_highlightTime);
        }
        lockedElements[chosenElement].Unlock();
        DataManager.Instance.SaveData();
        ChangeButtonState(buttons, true);
        if(lockedElements.Count == 1)
            onLastElementUnlocking?.Invoke();
        
    }

    private void ChangeButtonState(Button[] buttons, bool state)
    {
        foreach (var button in buttons)
        {
            button.interactable = state;
            if (button.TryGetComponent(out BuyButton buyButton))
            {
                buyButton.UpdateAvailability();
            }
        }
    }

    public void Resize(RectTransform targetSizes)
    {
        var contentSize = targetSizes.rect.size;
        var objectRect = GetComponent<RectTransform>();
        objectRect.sizeDelta = contentSize;
        objectRect.localPosition = Vector3.zero;
    }
}
