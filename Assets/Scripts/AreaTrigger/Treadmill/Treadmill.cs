using UnityEngine;

public class Treadmill : MonoBehaviour, IAreaTrigger
{
    [SerializeField] private SkinnedMeshRenderer _treadmillRenderer;

    private int _playerSpeedDecrease = 500;
    private int _playerButtDecrease = 5;

    private float _timer;
    private float _timeToDecrease = 0.5f;

    public void EnterArea(Player player)
    {
        player.PlayerAnimator.SetInsideTreadmill(true);
        player.PlayerMover.ChangeRunningSpeedBy(-_playerSpeedDecrease);
    }

    public void ExitArea(Player player)
    {
        player.PlayerAnimator.SetInsideTreadmill(false);
        player.PlayerMover.ChangeRunningSpeedBy(_playerSpeedDecrease);
        player.PlayerMover.OnTreadmill = false;
    }

    public void StayInArea(Player player)
    {
        _timer += Time.deltaTime;
        if (_timer >= _timeToDecrease)
        {
            _timer = 0;
            player.TryApplyDamage(_playerButtDecrease);
        }
        
    }

    private void Update()
    {
        _treadmillRenderer.material.mainTextureOffset += new Vector2(0, 2) * Time.deltaTime;
    }
}
