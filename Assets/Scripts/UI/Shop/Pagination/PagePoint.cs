using UnityEngine;

public class PagePoint : MonoBehaviour
{
    [SerializeField] private GameObject _activatedPoint;
    public void Activate()
    {
        _activatedPoint.SetActive(true);
    }

    public void Deactivate()
    {
        _activatedPoint.SetActive(false);
    }
}
