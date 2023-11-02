using UnityEngine;
using UnityEngine.UI;

public class Rewards : MonoBehaviour
{
    [SerializeField] private Button _getRewardsButton;
    [SerializeField] private Button _loseRewardButton;

    public Button GetRewardsButton => _getRewardsButton;
    public Button LoseRewardsButton => _loseRewardButton;

    private void OnEnable()
    {
        _loseRewardButton.gameObject.SetActive(false);
        Invoke("EnableLoseRewardButton", 3f);
    }

    private void EnableLoseRewardButton()
    {
        _loseRewardButton.gameObject.SetActive(true);
    }
}
