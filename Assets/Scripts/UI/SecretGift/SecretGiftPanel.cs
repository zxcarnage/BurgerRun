using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretGiftPanel : MonoBehaviour
{
    public Action ClosedSecretGiftPanel;

    [SerializeField] private BoxItemCoin _boxCoinPrefab;
    [SerializeField] private BoxItemSkin _boxSkinPrefab;
    [SerializeField] private Transform _groupParent;
    [SerializeField] private List<BoxItem> _boxes = new List<BoxItem>();
    [SerializeField] private Image _skinIcon;
    [SerializeField] private Button _skipButton;
    [SerializeField] private ShopSkins _skins;


    [SerializeField] private int _moneyRewardMin;
    [SerializeField] private int _moneyRewardMax;
    private Element _skinReward;

    private void OnEnable()
    {
        _skipButton.gameObject.SetActive(false);
        StartGiftPanel();
    }

    private void StartGiftPanel()
    {
        _skinReward = ServiceLocator.Shop.TryGetLockedCommon();
        ClearBoxes();

        SetUp(_skinReward);
        if (_skinReward)
            _skinIcon.sprite = _skinReward.Skin.SkinPreview;
        _skipButton.onClick.AddListener(Close);
    }

    private void ClearBoxes()
    {
        foreach (var box in _boxes)
        {
            Destroy(box.gameObject);
        }
        _boxes.Clear();
    }

    private void ActivateSkipButton()
    {
        _skipButton.gameObject.SetActive(true);
    }

    public void SetUp(Element skinReward)
    {
        if (skinReward == null)
        {
            for (int i = 0; i < 3; i++)
            {
                BoxItemCoin boxItem = (Instantiate(_boxCoinPrefab, _groupParent));
                boxItem.SetUpBox(UnityEngine.Random.Range(_moneyRewardMin, _moneyRewardMax));
                _boxes.Add(boxItem);
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                BoxItemCoin boxItem = (Instantiate(_boxCoinPrefab, _groupParent));
                boxItem.SetUpBox(UnityEngine.Random.Range(_moneyRewardMin, _moneyRewardMax));
                _boxes.Add(boxItem);
            }

            BoxItemSkin box3 = Instantiate(_boxSkinPrefab, _groupParent);
            box3.SetUpBox(skinReward);
            _boxes.Add(box3);
        }

        foreach (BoxItem box in _boxes)
        {
            box.transform.SetSiblingIndex(UnityEngine.Random.Range(0,3));
            box.BoxOpened += BoxOpenedHandler;
        }
    }

    private void BoxOpenedHandler()
    {
        foreach(BoxItem box in _boxes)
        {
            if (box.OpenedBox != true) 
            {
                box.ActivateRewardButton();
            }
        }
        ActivateSkipButton();
    }

    private void Close()
    {
        ClosedSecretGiftPanel?.Invoke();
        gameObject.SetActive(false);
    }
}
