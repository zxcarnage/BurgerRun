using System;
using TMPro;
using UnityEngine;

public class LevelCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    
    private LevelSpawner _spawner;

    private void Awake()
    {
        _spawner = FindObjectOfType<LevelSpawner>();
        OnLevelSpawned();
    }

    private void OnEnable()
    {
        _spawner.LevelSpawned += OnLevelSpawned;
    }

    private void OnDestroy()
    {
        _spawner.LevelSpawned -= OnLevelSpawned;
    }

    private void OnLevelSpawned()
    {
        string text = DataManager.Instance.Language == "ru" ? "УРОВЕНЬ " : "LEVEL ";
        _levelText.text = text + Convert.ToString(DataManager.Instance.CurrentLevel + 1);
    }
}
