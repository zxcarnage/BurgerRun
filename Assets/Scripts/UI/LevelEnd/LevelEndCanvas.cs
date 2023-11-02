using UnityEngine;

public class LevelEndCanvas : MonoBehaviour
{
    public bool CanShowSecretGiftPanel => (DataManager.Instance.CurrentLevel + 1) % 4 == 0;
    [SerializeField] private LevelSpawner _lvlSpawner;
    [SerializeField] private Currensy _currensy;
    [SerializeField] private WinPanel _winPanel;
    [SerializeField] private LosePanel _losePanel;
    [SerializeField] private SecretGiftPanel _secretGiftPanel;

    private float _multiplyer;

    private void Start()
    {
        _secretGiftPanel.ClosedSecretGiftPanel += ShowWinPanel;
    }

    private void ShowWinPanel()
    {
        ShowWinPanel(_multiplyer);
    }

    public void ShowWinPanel(float multiplyier)
    {
        ServiceLocator.InGameUI.HideLevel();
        _currensy.AddMoneyByMultiplyier(multiplyier);
        _winPanel.Initialize();
    }

    public void ShowLosePanel()
    {
        ServiceLocator.InGameUI.HideStat();
        _losePanel.Initialize();
    }

    public void ShowSecretGiftPanel(float multiplyier)
    {
        _multiplyer = multiplyier;
        ServiceLocator.InGameUI.HideStat();
        _secretGiftPanel.gameObject.SetActive(true);
    }

    public void BlockButtons()
    {
        _winPanel.BlockButtons();
    }

    public void UnlockButtons()
    {
        _winPanel.UnlockButtons();
    }

    public void LoadNext()
    {
        ServiceLocator.LevelSpawner.LoadNext();
    }
}
