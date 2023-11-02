using UnityEngine;

public class DeactivateZone : MonoBehaviour, IZone
{
    [SerializeField] private Canvas _canvas;
    public void AffectCanvas(Player player)
    {
        _canvas.gameObject.SetActive(false);
    }
}
