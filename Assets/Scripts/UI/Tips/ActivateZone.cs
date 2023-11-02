using UnityEngine;

public class ActivateZone : MonoBehaviour , IZone
{
    [SerializeField] private Canvas _canvas;
    public void AffectCanvas(Player player)
    {
        if(player.Health <= 40)
            _canvas.gameObject.SetActive(true);
    }
}
