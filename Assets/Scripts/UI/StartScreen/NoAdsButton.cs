using System;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class NoAdsButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private CatalogProduct _noadsProduct;
    [SerializeField] private PlayerStats _playerStats;

    private void Start()
    {
        if(_playerStats.NoAds)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(NoAdsButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(NoAdsButtonClicked);
    }

    private void NoAdsButtonClicked()
    {
#if UNITY_EDITOR
        ActivateNoAds();
        return;
#endif
        Billing.PurchaseProduct(_noadsProduct.id, (purchaseProductResponse) =>
        {
            Billing.ConsumeProduct(purchaseProductResponse.purchaseData.purchaseToken,ActivateNoAds);
            
        });
    }

    private void ActivateNoAds()
    {
        _playerStats.SetNoAds(true);
        DataManager.Instance.SaveData();
        gameObject.SetActive(false);
    }
}
