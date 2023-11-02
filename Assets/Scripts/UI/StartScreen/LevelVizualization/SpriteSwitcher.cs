using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    [SerializeField] private List<LevelVizualization> _levelImages;
    [SerializeField] private LevelSpawner _spawner;

    private void Start()
    {
        Init();
        _spawner.LevelSpawned += OnLevelSpawned;
    }
    

    private void OnDestroy()
    {
        _spawner.LevelSpawned -= OnLevelSpawned;
    }

    private void OnLevelSpawned()
    {
        var nextIcon = _levelImages.FirstOrDefault((x) => x.State == SpriteState.Disabled);
        if (nextIcon == default)
        {
            Init();
            return;
        }
        nextIcon.Activate();
    }

    private void Init()
    {
        InitStartImages();
        InitImages();
        InitText();
    }

    private void InitStartImages()
    {
        foreach (var image in _levelImages)
        {
            image.InitializeImage();
        }
    }

    private void InitText()
    {
        for (int i = 0; i < _levelImages.Count; i++)
        {
            _levelImages[i].InitializeText(i);
        }
    }

    private void InitImages()
    {
        var currentElementNum = (ServiceLocator.LevelSpawner.CurrentLevel) % 6;
        if (currentElementNum == 0)
            ResetImages();
        else
            for (int i = 0; i < currentElementNum + 1; i++)
            {
                _levelImages[i].Activate();
            }
    }

    private void ResetImages()
    {
        foreach (var levelSprite in _levelImages)
        {
            levelSprite.Deactivate();
        }
        _levelImages[0].Activate();
    }
}