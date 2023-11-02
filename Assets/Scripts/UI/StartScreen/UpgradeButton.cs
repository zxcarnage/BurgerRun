using System;
using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private UpgradeType _buttonType;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _currensyText;
    [SerializeField] private TMP_Text _distanceText;
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private Button _buyWithMoneyButton;
    [SerializeField] private Button _buyWithMoneyButton2;
    [SerializeField] private Button _buyWithYansButton;
    [SerializeField] private Button _buyWithAdButton;
    [SerializeField] private CatalogProduct _product;
    [SerializeField] private Currensy _money;
    [SerializeField] private float _startUpgradeAmount;
    [SerializeField] private float _incrementAmount;
    [SerializeField] private AudioClip _buySound;

    [Space(10f)] [Header("Upgrade Things")] 
    [SerializeField] private float _upgradeDistanceAmount;
    [SerializeField] private float _upgradePunchAmount;
    
    
    private float _currentUpgradeAmount;
    private int _upgradeLevel;

    private void Start()
    {
        CalculateLevels();
        _currentUpgradeAmount = _startUpgradeAmount + _upgradeLevel * _incrementAmount;
        TrySwitchMainButtons();
        TryDisable();
        UpdateText();
        ServiceLocator.LevelSpawner.LevelSpawned += TrySwitchMainButtons;
        _money.ValueUpdated += TrySwitchMainButtons;
        _buyWithMoneyButton.onClick.AddListener(OnUpgradeButtonClick);
        _buyWithMoneyButton2.onClick.AddListener(OnUpgradeButtonClick);
        _buyWithAdButton.onClick.AddListener(OnUpgradeWithAdButtonClick);
        _buyWithYansButton.onClick.AddListener(OnUpgradeWithYansButtonClick);
    }

    private void TryDisable()
    {
        if (!CanBeUpgraded())
        {
            _buyWithAdButton.interactable = false;
            _buyWithMoneyButton.interactable = false;
            _buyWithYansButton.interactable = false;
        }
    }

    private void CalculateUpgradeLevel(float maxValue, float startValue, float increment)
    {
        _upgradeLevel = Mathf.RoundToInt ((maxValue - startValue) / MathF.Round(increment, 1));
    }

    private void TrySwitchMainButtons()
    {
        if (_money.CoinAmount <= _currentUpgradeAmount)
            ChangeButtonState(false);
        else
            ChangeButtonState(true);
    }

    private void CalculateLevels()
    {
        if (_buttonType == UpgradeType.DistancePower)
        {
            CalculateUpgradeLevel(_stats.MaxDistance, _stats.StartingDistance, _upgradeDistanceAmount);
        }
        else if (_buttonType == UpgradeType.PunchPower)
        {
            CalculateUpgradeLevel(_stats.PushStrength, _stats.StartingPushStrength, _upgradePunchAmount);
        }
    }
    

    private void OnDestroy()
    {
        ServiceLocator.LevelSpawner.LevelSpawned -= TrySwitchMainButtons;
        _money.ValueUpdated -= TrySwitchMainButtons;
        _buyWithMoneyButton.onClick.RemoveListener(OnUpgradeButtonClick);
        _buyWithMoneyButton2.onClick.RemoveListener(OnUpgradeButtonClick);
        _buyWithAdButton.onClick.RemoveListener(OnUpgradeWithAdButtonClick);
        _buyWithYansButton.onClick.RemoveListener(OnUpgradeWithYansButtonClick);
    }

    private void ChangeButtonState(bool state)
    {
        _buyWithMoneyButton.gameObject.SetActive(state);
        _buyWithMoneyButton2.enabled = (state);
        _buyWithAdButton.gameObject.SetActive(!state);
    }

    private void OnUpgradeButtonClick()
    {
        if (!_money.TryDecreaseMoney(_currentUpgradeAmount))
            return;
        if (!CanBeUpgraded())
            return;
        Upgrade();
        TrySwitchMainButtons();
        SoundManager.Instance.PlaySound(_buySound);
    }

    private void OnUpgradeWithYansButtonClick()
    {
        if (!CanBeUpgraded())
            return;
#if !UNITY_EDITOR
        Billing.PurchaseProduct(_product.id, response => Upgrade());
        return;
#endif
        Upgrade();
    }

    private void OnUpgradeWithAdButtonClick()
    {
        if (!CanBeUpgraded())
            return;
#if !UNITY_EDITOR
        YandexManager.Instance.WatchRewardedVideo(Upgrade);
        TrySwitchMainButtons();
        return;
#endif
        Upgrade();
        TrySwitchMainButtons();
    }

    private void Upgrade()
    {
        _currentUpgradeAmount += _incrementAmount;
        switch (_buttonType)
        {
            case UpgradeType.PunchPower:
                UpgradePunch();
                break;
            case UpgradeType.DistancePower:
                UpgradeDistance();
                break;
        }
        _upgradeLevel++;
        Debug.Log(_upgradeLevel);
        DataManager.Instance.SaveData();
        UpdateText();
    }

    private bool CanBeUpgraded()
    {
        if (_buttonType == UpgradeType.DistancePower && _stats.MaxDistance >= 20f)
            return false;
        if (_buttonType == UpgradeType.PunchPower && _stats.PushStrength >= 135f)
            return false;
        return true;
    }

    private void UpgradePunch()
    {
        _stats.PushStrength += _upgradePunchAmount;
    }

    private void UpgradeDistance()
    {
        _stats.IncreaseDistance(_upgradeDistanceAmount);

    }

    private void UpdateText()
    {
        string level = DataManager.Instance.Language == "ru" ? "УРОВЕНЬ " : "LEVEL ";
        _levelText.text = level + (_upgradeLevel + 1);
        _currensyText.text = Convert.ToString(_currentUpgradeAmount);
        if (_distanceText)
        {
            string distance = DataManager.Instance.Language == "ru" ? "ДИСТАНЦИЯ X" : "DISTANCE X ";
            _distanceText.text = distance + _stats.MaxDistance.ToString("0.0");
        }
    }
}

enum UpgradeType
{
    PunchPower,
    DistancePower
}