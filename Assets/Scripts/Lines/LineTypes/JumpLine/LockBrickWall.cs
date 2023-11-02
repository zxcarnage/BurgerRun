using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBrickWall : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listOfObjectsToTurnOff = new List<GameObject>();

    public void DisableVisual()
    {
        foreach (var item in _listOfObjectsToTurnOff)
        {
            item.SetActive(false);
        }
    }
}
