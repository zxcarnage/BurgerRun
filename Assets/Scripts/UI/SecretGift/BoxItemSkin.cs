using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxItemSkin : BoxItem
{
    [SerializeField] private Image _skinIcon;
    
    private Element _skinElement;

    public void SetUpBox(Element skin)
    {
        _skinElement = skin;
        _skinIcon.sprite = skin.Skin.SkinPreview;
    }

    public override void OpenBox()
    {
        base.OpenBox();
        OpenSkin();
    }

    private void OpenSkin()
    {
        _skinElement.Unlock();
    }
}
