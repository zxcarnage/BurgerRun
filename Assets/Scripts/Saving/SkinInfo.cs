using System;
using UnityEngine;

[Serializable]
public class SkinInfo
{
    [SerializeReference] public Skin Skin;
    public SkinState SkinState;
    public string Name;

    public SkinInfo()
    {
        Skin = null;
        SkinState = SkinState.Closed;
    }
    
    public SkinInfo(SkinInfo from)
    {
        CopyParameters(from);
    }

    public void FromJson(string json)
    {
        CopyParameters(JsonUtility.FromJson<SkinInfo>(json));
    }

    private void CopyParameters(SkinInfo from)
    {
        Skin = from.Skin;
        SkinState = from.SkinState;
        Name = from.Name;
    }

    public void Unlock()
    {
        SkinState = SkinState.Opened;
    }
}
