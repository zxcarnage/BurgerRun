using UnityEngine;

public class Twerk : LevelVizualization
{
    [SerializeField] private Sprite _battleOff;
    [SerializeField] private Sprite _battleOn;

    public override void InitializeText(int elementOrder)
    {
        _text.gameObject.SetActive(false);
    }

    public override void InitializeImage()
    {
        _image.sprite = _battleOff;
    }

    public override void Activate()
    {
        base.Activate();
        _image.sprite = _battleOn;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _image.sprite = _battleOff;
    }
}
