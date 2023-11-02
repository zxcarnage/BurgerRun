using System;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class BuyWithYansButton : MonoBehaviour
{
    [SerializeField] private CatalogProduct _product;
    [SerializeField] private YansBuyingType _type;
    [SerializeField] private Button _buttonMain;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private Button _commonButton;
    [SerializeField] private Button _anotherButton;
    [SerializeField] private PaginationScroll _scroll;

    private PurchasedProduct[] _purchasedProducts;

    public void Init()
    {
        if (_scroll.TryGetLocked() == null)
        {
            DeactivateAllButtons();
            return;
        }
    }

    private void OnEnable()
    {
        _buttonMain.onClick.AddListener(OnBuyButtonClicked);
    }

    private void OnDisable()
    {
        _buttonMain.onClick.RemoveListener(OnBuyButtonClicked);
    }

    private void OnBuyButtonClicked()
    {
#if UNITY_EDITOR
        Buy();
        return;
#endif
        Billing.PurchaseProduct(_product.id, response =>
        {
            Billing.ConsumeProduct(response.purchaseData.purchaseToken, Buy);
        } );
    }

    private void Buy()
    {
        if (_scroll.TryGetLocked() != null)
        {
            switch (_type)
            {
                case YansBuyingType.Single:
                    BuyOne();
                    break;
                case YansBuyingType.All:
                    BuyAll();
                    break;
            }
            DataManager.Instance.SaveData();
        }
        else
            DeactivateAllButtons();
        
    }

    private void BuyOne()
    {
        _scroll.TryBuy(0, () => DeactivateAllButtons());
        if (_scroll.TryGetLocked() == null)
            DeactivateAllButtons();
    }

    private void BuyAll()
    {
        _scroll.UnlockAll();
        DataManager.Instance.SaveData();
        DeactivateAllButtons();
    }

    private void DeactivateAllButtons()
    {
        gameObject.GetComponent<Button>().interactable = false;
        _commonButton.interactable = false;
        _rewardButton.interactable = false;
        _anotherButton.interactable = false;
    }
}

enum YansBuyingType
{
    Single,
    All
}
