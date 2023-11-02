using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New skins", menuName = "Shop Skins", order = 0)]
public class ShopSkins : ScriptableObject
{
    [SerializeField] private SkinPages _skinPages;

    public SkinPages SkinPages => _skinPages;
    public int UnlockedCount => UnlockedSkins();

    public void CreateNew(SkinPages startSkinPages)
    {
        _skinPages = new SkinPages(startSkinPages);
    }

    private int UnlockedSkins()
    {
        int count = 0;
        var pages = _skinPages.PagesList;
        foreach (var page in pages)
        {
            count += page.Skins.FindAll(x => x.SkinState == SkinState.Opened).Count;
        }
        return count;
    }

    public void UnlockAll()
    {
        Debug.Log("In unlock all");
        foreach (var page in _skinPages.PagesList)
        {
            foreach (var skinInfo in page.Skins)
            {
                skinInfo.SkinState = SkinState.Opened;
            }
        }
    }

    public void UnlockRandom()
    {
        _skinPages.UnlockRandom();
    }

    public Skin GetRandomLockedSkin()
    {
        Skin skin;
        var pages = _skinPages.PagesList;
        foreach (var page in pages)
        {
            skin = page.Skins.FirstOrDefault(n => n.SkinState == SkinState.Closed).Skin;
            if (skin != null)
                return skin;
        }
        return null;
         

    }
}