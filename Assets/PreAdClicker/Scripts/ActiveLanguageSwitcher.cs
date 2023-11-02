using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveLanguageSwitcher : MonoBehaviour
{
    [SerializeField] private string ru;
    [SerializeField] private string en;
    [SerializeField] private TMP_Text text;

    private string levelLabel;

    private void Awake()
    {
        switch (DataManager.Instance.Language)
        {
            case "ru":
                levelLabel = ru;
                break;

            case "com":
                levelLabel = en;
                break;
        }
        
        UpdateValue(0);
    }

    public void UpdateValue(int sec)
    {
        text.text = $"{levelLabel} {sec}";
    }    
}
