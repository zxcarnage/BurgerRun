using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class SkinPages
{
    public List<SkinList> PagesList;

    public List<Skin> SkinsForReference;
    public List<SkinList> SkinListForReference;

    public SkinPages(SkinPages from)
    {
        PagesList = new List<SkinList>();
        foreach (var fromSkinList in from.PagesList)
        {
            var newSkinList = new SkinList(fromSkinList);
            PagesList.Add(newSkinList);
        }
    }

    public void UnlockRandom()
    {
        List<SkinInfo> lockedElements = new List<SkinInfo>(); 
        foreach (var page in PagesList)
        {
            Debug.Log("Page " + page);
            lockedElements.AddRange(page.Skins);
        }

        if (lockedElements.Count != 0)
            lockedElements[UnityEngine.Random.Range(0, lockedElements.Count - 1)].SkinState = SkinState.Opened;
    }

    public string ToJson()
    {
        List<string> skinListJson = new List<string>();
        foreach (var page in PagesList)
        {
            var json = page.ToJson();
            skinListJson.Add(json);
        }
        return JsonHelper.ToJson(skinListJson.ToArray());
    }

    public void FromJson(string json)
    {
        PagesList = new List<SkinList>();
        List<string> skinListJson = JsonHelper.FromJson<string>(json).ToList();
        for (int i = 0; i < skinListJson.Count; i++)
        {
            string listJson = skinListJson[i];
            SkinList newSkinList = new SkinList();
            newSkinList.FromJson(listJson);
            RepairSkinList(newSkinList, i);
            PagesList.Add(newSkinList);
        }
    }

    private void RepairSkinList(SkinList skinList, int indexOfSkinList)
    {
        for (int i = 0; i < skinList.Skins.Count; i++)
        {
            skinList.Skins[i].Skin = SkinListForReference[indexOfSkinList].Skins[i].Skin;
        }
        //foreach (var item in skinList.Skins)
        //{
        //    if (item.Skin == null || SkinsForReference.FirstOrDefault(n => n.name == item.Name).GetInstanceID() != item.Skin.GetInstanceID())
        //    {
        //        item.Skin = SkinsForReference.FirstOrDefault(n => n.name == item.Name);
        //    }
        //}
    }

    
}
