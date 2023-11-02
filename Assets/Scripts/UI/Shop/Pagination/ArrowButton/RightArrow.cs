using UnityEngine;
using UnityEngine.EventSystems;

public class RightArrow : Arrow
{
    private void Start()
    {
        _disablePage = _scroll.MaxPage;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Scroll(Direction.Right);
    }
}
