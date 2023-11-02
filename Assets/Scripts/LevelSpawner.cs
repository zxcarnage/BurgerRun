using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;
    [SerializeField] private LoadScreen _loadScreen;
    [SerializeField] private LevelEndCanvas _endCanvas;
    [SerializeField] private InterstitialAdsManager _manager;

    public event Action LevelSpawned;
    public int CurrentLevel => _currentLvlNum;

    private int _currentLvlNum;
    private Level _intantiatedLevel;
    private List<Level> _shortLevels;
    private List<Level> _longLevels;

    private void Awake()
    {
        ServiceLocator.LevelSpawner = this;
    }

    private void OnEnable()
    {
        ServiceLocator.SetEndCanvas(_endCanvas);
        _currentLvlNum = DataManager.Instance.CurrentLevel;
        if (_currentLvlNum > 40)
            _currentLvlNum = Random.Range(30, 40);
        SpawnLevel(_currentLvlNum);
    }

    private void Start()
    {
        _shortLevels = _levels.FindAll((x) => x.gameObject.TryGetComponent(out ShortLevel shortLevel));
        _longLevels = _levels.FindAll((x) => x.gameObject.TryGetComponent(out LongLevel longLevel));
    }

    private void SpawnLevel(int levelNum)
    {
        Level newLevel;
        if (_currentLvlNum >= _levels.Count)
        {
            newLevel = (_currentLvlNum + 1) % 3 == 0
                ? _shortLevels[Random.Range(0, _shortLevels.Count - 1)]
                : _longLevels[Random.Range(0, _longLevels.Count - 1)];
        }
        else
            newLevel = _levels[levelNum];
        _intantiatedLevel = Instantiate(newLevel);
        GameManager.Instance.ResetSlider();
        LevelSpawned?.Invoke();
    }

    public void LoadNext()
    {
        Destroy(_intantiatedLevel.gameObject);
        Debug.Log("Destroy");
        _currentLvlNum++;
        Debug.Log("Current lvl num");
        DataManager.Instance.CurrentLevel += 1;
        Debug.Log("Current lvl");
        DataManager.Instance.SaveData();
        Debug.Log("Save data");
#if UNITY_EDITOR
        ShowAdsAndSpawnLevel();
        //SpawnLevel(_currentLvlNum);
        return;
#endif
        ShowAdsAndSpawnLevel();
    }

    public void Restart()
    {
        Debug.Log(_intantiatedLevel.name);
        Destroy(_intantiatedLevel.gameObject);
        DataManager.Instance.SaveData();
#if UNITY_EDITOR
        SpawnLevel(_currentLvlNum);
        return;
#endif
        ShowAdsAndSpawnLevel();
    }

    private void ShowAdsAndSpawnLevel()
    {
        var isPaused = AudioListener.pause;
        SpawnLevel(_currentLvlNum);
        Debug.Log("Spawn level");

        PreAdScreen.Instance.ShowAdClicker();
        Debug.Log("Try show ad");
    }
    
}
