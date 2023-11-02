using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SkinList
{
    public List<SkinInfo> Skins;

    public SkinList()
    {
        Skins = new List<SkinInfo>();
    }

    public string ToJson()
    {
        List<string> skinInfos = new List<string>();
        foreach (var skinInfo in Skins)
        {
            var json = JsonUtility.ToJson(skinInfo);
            skinInfos.Add(json);
        }

        var arr = skinInfos.ToArray();
        var endJson = JsonHelper.ToJson(arr);
        return endJson ;
    }

    public void FromJson(string json)
    {
        Skins = new List<SkinInfo>();
        var skinInfos = JsonHelper.FromJson<string>(json);
        foreach (var skinInfo in skinInfos)
        {
            SkinInfo newSkinInfo = new SkinInfo();
            newSkinInfo.FromJson(skinInfo);
            Skins.Add(newSkinInfo);
        }
    }
    
    public SkinList(SkinList from)
    {
        LoadDataInList(from.Skins);
    }

    private void LoadDataInList(List<SkinInfo> from)
    {
        Skins = new List<SkinInfo>();
        foreach (var fromSkinInfo in from)
        {
            var newSkinInfo = new SkinInfo(fromSkinInfo);
            Skins.Add(newSkinInfo);
        }
    }
}
