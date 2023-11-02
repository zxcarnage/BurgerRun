using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TwerkPlayerSkinSwitcher : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _bodySkin;
    [SerializeField] private GameObject _standartRig;
    [SerializeField] private PlayerSkin _currentSkin;
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
        _currentSkin.SkinChanged += OnSkinChanged;
    }

    private void OnDisable()
    {
        _currentSkin.SkinChanged -= OnSkinChanged;
    }

    private void OnSkinChanged(Skin newSkin)
    {
        Debug.Log("In on Skin changed");
        InitializeParts();
        Switch(newSkin);
    }

    private void InitializeParts()
    {
        Debug.Log("In Initialize parts");
        _currentRig = transform.GetChild(0).gameObject;
        TryInitializeSkinnedRenderers();
    }

    public void Initialize()
    {
        InitializeParts();
        Switch(_currentSkin.Skin);
    }
    
    private void Switch(Skin newSkin)
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
        Debug.Log("Before ChangeModel(hair) " + _hairModels);
        ChangeModel(_hairModels,newSkin.HairMaterial, newSkin, newSkin.HairCounter);
        Debug.Log("Before ChangeModel(boots) " + _bootsModels);
        ChangeModel(_bootsModels, newSkin.BootsMaterial,newSkin, newSkin.BootsNum);
        _bodySkin.material = newSkin.Body;
    }

    private void ChangeModel(List<SkinnedMeshRenderer> meshes, Material material, Skin newSkin, int counter)
    {
        var newModel = SwitchListObject(meshes, newSkin, counter);
        newModel.material = material;
    }

    private void SwitchRig(Skin newSkin)
    {
        GameObject newRig;
        if (!newSkin.SkinRig)
            newRig = _standartRig;
        else
            newRig = newSkin.SkinRig;
        var changeableRig = transform.GetChild(0);
        Destroy(changeableRig.gameObject);
        Debug.Log(changeableRig + "Destroyed");
        var rigAnimator = AddOrGetComponent<Animator>(newRig);
        rigAnimator.avatar = _avatar;
        rigAnimator.runtimeAnimatorController = _controller;
        AddOrGetComponent<TwerkParticipantAnimationReceiver>(newRig);
        _currentRig = Instantiate(newRig, transform.position, Quaternion.Euler(0, -90, 0), transform);
        Debug.Log(_currentRig + "Instantiated");
        _skinState = SkinState.Legendary;
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
        Debug.Log("In TryInitializeSkinnedRenderers");
        if (_currentRig.TryGetComponent(out CommonSkinPartsInitializer parts) && (_hairModels == null || _bootsModels == null)) 
        {
            _hairModels = new List<SkinnedMeshRenderer>();
            _bootsModels = new List<SkinnedMeshRenderer>();
            parts.Initialize();
            FullfillSkinnedMesh(_hairModels, parts.HairList);
            FullfillSkinnedMesh(_bootsModels, parts.ShoesList);
            Debug.Log(_hairModels);
            Debug.Log(_bootsModels);
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
        Debug.Log(_currentSkin);
        Debug.Log("List is " + list + " (Skin Twerk)");
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
