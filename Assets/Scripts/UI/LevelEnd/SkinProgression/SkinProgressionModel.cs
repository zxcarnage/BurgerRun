using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class SkinProgressionModel : MonoBehaviour
{
    
    [SerializeField] private SkinnedMeshRenderer _bodySkin;
    [SerializeField] protected PlayerSkin _currentSkin;

    [Space(15f)] [Header("Shader properties")] 
    [SerializeField] private Transform _pointForShaderFrom;
    [SerializeField] private Transform _pointForShaderTo;
    [SerializeField] private UnityEngine.Camera _canvasCamera;
    [SerializeField] private float _bottomValue;
    [SerializeField] private float _highValue;
    [SerializeField] private Material _material;
    [SerializeField] private float _animationDuration;

    private Skin _previousSkin;
    private SkinState _skinState;
    private GameObject _currentRig;

    private List<SkinnedMeshRenderer> _hairModels;
    private List<SkinnedMeshRenderer> _bootsModels;

    private float _currentFullfillPercent = 0f;
    private float _currentBottomValue;
    private Element _currentLockedElement;
    private Sequence _sequence;

    private bool TrySetModel(Action onSuccessCallback)
    {
        bool success = true;
        //Обработка покупки _currentLockedElement
        if (_currentSkin.Skin == null || _currentLockedElement == null || _currentLockedElement.ElementState != State.Locked)
            success = TryFindNewSkin();
        if (success)
        {
            SetModel(_currentSkin.Skin);
            onSuccessCallback?.Invoke();
        }
        return success;
    }

    private bool TryFindNewSkin()
    {
        _currentLockedElement = ServiceLocator.Shop.TryGetLockedCommon();
        Debug.Log("Current Locked Element = " + _currentLockedElement);
        if (_currentLockedElement)
        {
            _currentSkin.ChangeSkin(_currentLockedElement.Skin);
            return true;
        }
        else
        {
            gameObject.SetActive(false);
            return false;
        }
    }
    
    private void SetModel(Skin skin)
    {
        Switch(skin);
        ChangeSizes();
    }

    private void ChangeSizes()
    {
        transform.localScale = new Vector3(2, 2, 2);
    }

    public void Init(float fullfillPercent, Action onSuccessCallback = null)
    {
        InitializeShaderValues();
        if (_currentBottomValue == 0f)
            _currentBottomValue = _bottomValue;
        if(_currentRig == null)
            _currentRig = gameObject;
        TryInitializeSkinnedRenderers();
        if(!TrySetModel(onSuccessCallback))
            return;
        _currentFullfillPercent += fullfillPercent;
        StartCoroutine(AnimateFullfill(_currentFullfillPercent));
    }

    private void InitializeShaderValues()
    {
        _bottomValue = _canvasCamera.WorldToScreenPoint(_pointForShaderFrom.position).y;
        _highValue = _canvasCamera.WorldToScreenPoint(_pointForShaderTo.position).y;
    }

    private IEnumerator AnimateFullfill(float fullfillPercent)
    {
        float elapsedTime = 0f;
        var targetValue = Mathf.Lerp(_bottomValue, _highValue, fullfillPercent);
        var startValue = _currentBottomValue;
        ServiceLocator.GetEndCanvas().BlockButtons();
        while (elapsedTime <= _animationDuration)
        {
            _currentBottomValue = Mathf.Lerp(startValue, targetValue, elapsedTime / _animationDuration);
            elapsedTime += Time.deltaTime;
            _material.SetVector("_ClippingCentre", new Vector4(0, _currentBottomValue, 0, 0));
            yield return null;
        }

        _currentBottomValue = targetValue;
        ServiceLocator.GetEndCanvas().UnlockButtons();
    }
    
    private void Switch(Skin newSkin)
    {
        ChangeModel(_hairModels,newSkin.HairMaterial, newSkin, newSkin.HairCounter);
        ChangeModel(_bootsModels, newSkin.BootsMaterial,newSkin, newSkin.BootsNum);
        _bodySkin.sharedMaterials[0].mainTexture = newSkin.Body.mainTexture;
        _currentSkin.ChangeSkin(newSkin);
    }

    public void OpenNewSkin()
    {
        ResetSkinProgression();
        _currentLockedElement.Unlock();
    }

    public void ResetSkinProgression()
    {
        ResetSkin();
        transform.rotation = Quaternion.identity;
        _sequence.Kill();
        _currentFullfillPercent = 0f;
        _currentBottomValue = _bottomValue;
        _material.SetVector("_ClippingCentre", new Vector4(0, _currentBottomValue, 0, 0));
    }

    private void ResetSkin()
    {
        _currentSkin.ChangeSkin(null);
    }

    public void Animate()
    { 
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DORotate(new Vector3(0f, 360f, 0f), 4f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear));
        _sequence.Play();
    }
    
    
    private void ChangeModel(List<SkinnedMeshRenderer> meshes, Material material, Skin newSkin, int counter)
    {
        var newModel = SwitchListObject(meshes, newSkin, counter);
        newModel.sharedMaterials[0].mainTexture = material.mainTexture;
    }
    

    private void TryInitializeSkinnedRenderers()
    {
        if (_currentRig.TryGetComponent(out CommonSkinPartsInitializer parts) && _hairModels == null || _bootsModels == null)
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
        Debug.Log(_currentSkin);
        Debug.Log("List is " + list + " (Skin Switcher Model)");
        list.SingleOrDefault(x => x.gameObject.activeSelf)?.gameObject.SetActive(false);//!!!!!!
        var newModel = list[counter - 1];
        newModel.gameObject.SetActive(true);
        return newModel;
    }
}
