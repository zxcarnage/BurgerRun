using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _levelCounter;
    [SerializeField] private GameObject _moneyCounter;
    [SerializeField] private ShopCanvas _shopCanvas;
    [SerializeField] private Button _shopButton;
    [SerializeField] private LevelSpawner _levelSpawner;

    private void OnEnable()
    {
        _levelSpawner.LevelSpawned += OnLevelSpawned;
        _shopButton.onClick.AddListener(OnShopButtonClicked);
    }

    private void Start()
    {
        Enable();
        ServiceLocator.InGameUI = this;
    }

    private void OnDisable()
    {
        _levelSpawner.LevelSpawned -= OnLevelSpawned;
        _shopButton.onClick.RemoveListener(OnShopButtonClicked);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Disable();
    }
    
    private void Enable()
    {
        ServiceLocator.SpawnPoint.Player.BlockMovement();
        ChangeState(true);
        ShowStat();
    }

    private void Disable()
    {
        ChangeState(false);
        ShowStat();
        ServiceLocator.SpawnPoint.Player.UnlockMovement();
    }

    private void ChangeState(bool isActive)
    {
        if(isActive)
            GameManager.Instance.DisableSlider();
        else
            GameManager.Instance.EnableSlider();
        _startPanel.SetActive(isActive);
    }

    private void OnLevelSpawned()
    {
        Enable();
    }

    private void OnShopButtonClicked()
    {
        _shopCanvas.Show();
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowStat()
    {
        _moneyCounter.SetActive(true);
        _levelCounter.SetActive(true);
    }

    public void HideLevel()
    {
        _levelCounter.SetActive(false);
    }

    public void HideStat()
    {
        _moneyCounter.SetActive(false);
        _levelCounter.SetActive(false);
    }
}
