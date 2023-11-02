using UnityEngine;

public class PlayerAnimationEventReciever : MonoBehaviour
{
    private PlayerAnimator _playerAnimator;

    private void Awake()
    {
        _playerAnimator = ServiceLocator.Player.GetComponent<PlayerAnimator>();
    }

    public void StandUp()
    {
        _playerAnimator.StandUp();
    }

    public void SetIdle()
    {
        _playerAnimator.SetIdle();
    }
    
    public void InAir()
    {
        _playerAnimator.InAir();
    }

    public void UnlockMovement()
    {
        _playerAnimator.UnlockMovement();
    }
    
}
