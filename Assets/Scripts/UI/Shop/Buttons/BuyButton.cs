using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BuyButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private ShopSkins _skins;
    [SerializeField] private Button _adsButton;
    [SerializeField] private Button _buyAllButton;
    [SerializeField] private Button _buySingleButton;
    [SerializeField] private float _startCost;
    [SerializeField] private float _progressionAmount;
    [SerializeField] private PaginationScroll _scroll;
    [SerializeField] private AudioClip _buySound;
    [SerializeField] private Currensy _playerMoney;

    private Button _button;
    private float _currentCost;

    private float _currentUnlockCost => _scroll.GetCurrentPageCost() + _currentCost + _startCost;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Init()
    {
        TryDeactivate();
        CalculateStartCost();
        UpdateCost();
        UpdateAvailability();
    }

    private void TryDeactivate()
    {
        if (_scroll.TryGetLocked() == null || _scroll.TryGetLocked().Count == 1 || _scroll.TryGetLockedOnPage(1) == null)
        {
            ChangeButtonsState(false);
        }
    }

    private void ChangeButtonsState(bool state)
    {
        _adsButton.interactable = state;
        _buyAllButton.interactable = state;
        _buySingleButton.interactable = state;
        UpdateAvailability();
    }

    private void CalculateStartCost()
    {
        _startCost = _skins.UnlockedCount * _progressionAmount;
    }

    private void OnEnable()
    {
        _scroll.PageChanged += OnPageChanged;
        _button.onClick.AddListener(OnBuyButtonClicked);
        _buySingleButton.onClick.AddListener(TryDeactivate);
        _buyAllButton.onClick.AddListener(TryDeactivate);
        _playerMoney.ValueUpdated += UpdateAvailability;
        UpdateAvailability();
    }

    private void OnDisable()
    {
        _scroll.PageChanged -= OnPageChanged;
        _button.onClick.RemoveListener(OnBuyButtonClicked);
        _buySingleButton.onClick.RemoveListener(TryDeactivate);
        _buyAllButton.onClick.RemoveListener(TryDeactivate);
        _playerMoney.ValueUpdated -= UpdateAvailability;
    }

    public void UpdateAvailability()
    {
        gameObject.GetComponent<Button>().interactable = _playerMoney.CoinAmount >= _currentUnlockCost;
    }

    private void OnPageChanged(int currentPage)
    {
        Debug.Log("current page \n" + currentPage);
        TryChangeState(currentPage);
        UpdateCost();
    }

    private void TryChangeState(int currentPage)
    {
        if(_scroll.TryGetLockedOnPage(currentPage) != null)
            ChangeButtonsState(true);
        else
            ChangeButtonsState(false);
    }

    private void UpdateCost()
    {
        _costText.text = Convert.ToString(_currentUnlockCost);
    }

    private void OnBuyButtonClicked()
    {
        if (!_scroll.TryBuy(_currentUnlockCost, () => ChangeButtonsState(false)))
            return;
        TryDeactivate();
        _currentCost += _progressionAmount;
        UpdateCost();
        SoundManager.Instance.PlaySound(_buySound);

    }
}
