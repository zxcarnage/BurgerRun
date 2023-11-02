using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Element : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _cellPreview;
    [SerializeField] private Sprite _skinSelected;
    [SerializeField] private Sprite _cellUlocked;
    [SerializeField] private Image _lockImage;
    [SerializeField] private Sprite _lockHighlighted;
    [SerializeField] private PlayerSkin _currentSkin;
    
    public Skin Skin => _skinInfo.Skin;
    private SkinInfo _skinInfo;
    private Sprite _lockUnhighlighted;
    private State _elementState;
    private Image _cellLockedImage;
    public State ElementState => _elementState;
    public event Action<Element> ElementChosen;

    private void Awake()
    {
        _cellLockedImage = GetComponent<Image>();
        _lockUnhighlighted = _lockImage.sprite;
    }

    public void Initialize(SkinInfo skinInfo)
    {
        _skinInfo = skinInfo;
        if(skinInfo.SkinState == SkinState.Opened)
            Unlock();
        else
            _elementState = State.Locked;
        if (skinInfo.Skin.Chosen())
            Choose();
    }

    public void Highlight()
    {
        _lockImage.sprite = _lockHighlighted;
    }

    public void UnHighlight()
    {
        _lockImage.sprite =_lockUnhighlighted ;
    }

    public void Choose()
    {
        _cellLockedImage.sprite = _skinSelected;
        _elementState = State.Active;
        ServiceLocator.Player.SwitchSkin(Skin);
    }

    public void UnChoose()
    {
        _cellLockedImage.sprite = _cellUlocked;
        _elementState = State.Disabled;
    }

    public void Unlock()
    {
        Debug.Log("Unlocking");
        _elementState = State.Disabled;
        _skinInfo.Unlock();
        _cellPreview.SetActive(true);
        _lockImage.gameObject.SetActive(false);
        var cellImage = _cellPreview.GetComponent<Image>();
        cellImage.sprite = Skin.SkinPreview;
        _cellLockedImage.sprite = _cellUlocked;
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_elementState == State.Locked || _skinInfo.SkinState == SkinState.Closed)
            return;
        ElementChosen?.Invoke(this);
    }
}

public enum State
{
    Active,
    Disabled,
    Locked
}
