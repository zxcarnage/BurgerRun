using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkinSwitcher : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _bodySkin;
    [SerializeField] private GameObject _standartRig;
    [SerializeField] protected PlayerSkin _currentSkin;
    [SerializeField] private RuntimeAnimatorController _controller;
    [SerializeField] private Avatar _avatar;

    private Skin _previousSkin;
    private SkinState _skinState;
    private GameObject _currentRig;

    private List<SkinnedMeshRenderer> _hairModels;
    private List<SkinnedMeshRenderer> _bootsModels;
    public event Action<GameObject> RigChanged;

    private void OnEnable()
    {
        ServiceLocator.LevelSpawner.LevelSpawned += OnLevelSpawned;
    }

    private void OnDisable()
    {
        ServiceLocator.LevelSpawner.LevelSpawned -= OnLevelSpawned;
    }

    private void OnLevelSpawned()
    {
        _currentRig = FindRig().gameObject;
        TryInitializeSkinnedRenderers();
        Switch(_currentSkin.Skin);
    }

    public void Switch(Skin newSkin)
    {
        if (newSkin.SkinRig)
        {
            SwitchRig(newSkin);
            return;
        }
        if (_skinState == SkinState.Legendary)
            SwitchFromLegendary(newSkin);
        else
            SwitchCommonOnCommon(newSkin);
        
    }

    private void SwitchCommonOnCommon(Skin newSkin)
    {
        ChangeModel(_hairModels,newSkin.HairMaterial, newSkin, newSkin.HairCounter);
        ChangeModel(_bootsModels, newSkin.BootsMaterial,newSkin, newSkin.BootsNum);
        _bodySkin.material = newSkin.Body;
        _currentSkin.ChangeSkin(newSkin);
    }

    private void ChangeModel(List<SkinnedMeshRenderer> meshes, Material material, Skin newSkin, int counter)
    {
        var newModel = SwitchListObject(meshes, newSkin, counter);
        newModel.material = material;
    }

    private Transform FindRig()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.TryGetComponent(out JumpingClone clone))
                return transform.GetChild(i);
        }

        return null;
    }

    private void SwitchRig(Skin newSkin)
    {
        GameObject newRig;
        if (!newSkin.SkinRig)
            newRig = _standartRig;
        else
            newRig = newSkin.SkinRig;
        var changeableRig = FindRig();
        Destroy(changeableRig.gameObject);
        var rigAnimator = AddOrGetComponent<Animator>(newRig);
        rigAnimator.avatar = _avatar;
        rigAnimator.runtimeAnimatorController = _controller;
        AddOrGetComponent<PlayerAnimationEventReciever>(newRig);
        _currentRig = Instantiate(newRig, transform);
        _skinState = SkinState.Legendary;
        _currentSkin.ChangeSkin(newSkin);
        RigChanged?.Invoke(_currentRig);
    }

    private static T AddOrGetComponent<T>(GameObject gameObject) where T : Component
    {
        return gameObject.TryGetComponent<T>(out var outComponent)
            ? outComponent
            : gameObject.AddComponent<T>();
    }

    private void SwitchFromLegendary(Skin newSkin)
    {
        SwitchRig(newSkin);
        TryInitializeSkinnedRenderers();
        _skinState = SkinState.Common;
        SwitchCommonOnCommon(newSkin);
    }

    private void TryInitializeSkinnedRenderers()
    {
        if (_currentRig.TryGetComponent(out CommonSkinPartsInitializer parts))
        {
            _hairModels = new List<SkinnedMeshRenderer>();
            _bootsModels = new List<SkinnedMeshRenderer>();
            parts.Initialize();
            FullfillSkinnedMesh(_hairModels, parts.HairList);
            FullfillSkinnedMesh(_bootsModels, parts.ShoesList);
            _bodySkin = parts.Body;
        }
    }

    private void FullfillSkinnedMesh(List<SkinnedMeshRenderer> meshRenderers, List<GameObject> parts)
    {
        foreach (var part in parts)
        {
            meshRenderers.Add(part.GetComponent<SkinnedMeshRenderer>());
        }
    }

    private SkinnedMeshRenderer SwitchListObject(List<SkinnedMeshRenderer> list, Skin newSkin, int counter)
    {
        Debug.Log("Player Switch List Object = " + list);
        list.SingleOrDefault(x => x.gameObject.activeSelf)?.gameObject.SetActive(false);
        var newModel = list[counter - 1];
        newModel.gameObject.SetActive(true);
        return newModel;
    }

    private enum SkinState
    {
        Common,
        Legendary
    }
}
