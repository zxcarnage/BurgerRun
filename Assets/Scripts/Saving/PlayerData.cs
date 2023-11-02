using System;

[Serializable]
public class PlayerData
{
    public float PushStrenght;
    public float MaxDistance;
    public bool NoAds;
    public float Money;
    public int CurrentLevel;
    public Skin PlayerSkin;
    public int MoneyProgresion;
    public PlayerData(PlayerStats stats, Currensy currensy, int level, Skin playerSkin, int moneyProgresion = 0)
    {
        PushStrenght = stats.PushStrength;
        MaxDistance = stats.MaxDistance;
        Money = currensy.CoinAmount;
        NoAds = stats.NoAds;
        PlayerSkin = playerSkin;
        CurrentLevel = level;
        MoneyProgresion = moneyProgresion;
    }

    public PlayerData()
    {
        PushStrenght = 69;
        MaxDistance = 6.8f;
        Money = 0;
        CurrentLevel = 0;
        NoAds = false;
    }
}
