using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsTimer : MonoBehaviour
{
    [SerializeField] protected float _time = 120;
    [SerializeField] private Button _claimButton;
    [SerializeField] protected Image _fill;
    [SerializeField] private Text _moneyText;
    [SerializeField] protected Text _remainingTimeText;
    [SerializeField] private Currensy _money;
    [SerializeField] private PlayerStats _playerStats;


    [SerializeField] private int[] _moneyAmountProgression;

    protected float _timer = 0;
    protected bool _unlocked;
    private int _moneyAmount;
    private int _moneyAmountIndex = 0;

    private void OnEnable()
    {
        _claimButton.onClick.AddListener(ClaimReward);
    }

    private void OnDisable()
    {
        _claimButton.onClick.RemoveListener(ClaimReward);
    }

    protected virtual void ClaimReward()
    {
        _money.AddMoney(_moneyAmount);

        NextMoneyProgression();
        SetUp(_moneyAmountIndex);
        ResetTimer();
    }

    protected void NextMoneyProgression()
    {
        _moneyAmountIndex++;
        _playerStats.SetMoneyProgression(_moneyAmountIndex);
        DataManager.Instance.SaveData();
    }

    protected virtual void Awake()
    {
        ResetTimer();
        SetUp(_playerStats.MoneyProgression);
    }

    protected void ResetTimer()
    {
        _claimButton.gameObject.SetActive(false);
        _timer = 0;
        _unlocked = false;
        _remainingTimeText.gameObject.SetActive(true);
    }

    

    public void SetUp(int moneyAmountIndex)
    {
        _moneyAmountIndex = moneyAmountIndex;
        if (moneyAmountIndex >= _moneyAmountProgression.Length)
            _moneyAmount = _moneyAmountProgression[_moneyAmountProgression.Length - 1];
        else
            _moneyAmount = _moneyAmountProgression[moneyAmountIndex];
        _moneyText.text = _moneyAmount.ToString();
    }

    


    private void Update()
    {
        if (_unlocked) return;

        _timer += Time.deltaTime;
        SetFillAndText();
        if (_timer > _time)
        {
            UnlockTimer();
        }
    }

    private void SetFillAndText()
    {
        _fill.fillAmount = 1 - _timer / _time;
        _remainingTimeText.text = FormatFloatToTime(_time - _timer);
    }

    protected virtual void UnlockTimer()
    {
        _claimButton.gameObject.SetActive(true);
        _fill.fillAmount = 0;
        _unlocked = true;
        _remainingTimeText.gameObject.SetActive(false);
    }


    private string FormatFloatToTime(float value)
    {
        int minutes = (int)Math.Floor(value / 60);
        int seconds = (int)value % 60;
        return minutes + ":" + seconds.ToString("D2");
    }
}
