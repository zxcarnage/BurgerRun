using System;
using UnityEngine;

public class Common : LevelVizualization
{
    [SerializeField] private Sprite _commonOff;
    [SerializeField] private Sprite _commonOn;
    
    public override void InitializeText(int elementOrder)
    {
        SetOrder(elementOrder);
    }

    public override void InitializeImage()
    {
        _image.sprite = _commonOff;
    }

    private void SetOrder(int elementOrder)
    {
        var levelCount = (ServiceLocator.LevelSpawner.CurrentLevel);
        var remainder =  levelCount % 6;
        if (levelCount is >= 1 and <= 6)
            levelCount = 1;
        else if (remainder != 1)
            levelCount -= (remainder - 1);
        Debug.Log(name + " " + (levelCount + elementOrder));
        _text.text = Convert.ToString(levelCount + elementOrder);
    }

    public override void Activate()
    {
        base.Activate();
        Debug.Log(name + " Activated" );
        _image.sprite = _commonOn;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Debug.Log(name + " Deactivated" );
        _image.sprite = _commonOff;
    }
}
