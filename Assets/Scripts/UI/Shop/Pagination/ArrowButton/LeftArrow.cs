using UnityEngine.EventSystems;

public class LeftArrow : Arrow
{
    private void Awake()
    {
        _disablePage = 1;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Scroll(Direction.Left);
    }
}
