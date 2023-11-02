using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour, IAreaTrigger
{
    [SerializeField] private float _strengthOfWind = 5f;

    private Vector3 _directionOfWind => transform.forward;

    public void EnterArea(Player player)
    {
        
    }

    public void ExitArea(Player player)
    {
        
    }

    public void StayInArea(Player player)
    {
        player.transform.position += (-_directionOfWind.z * Vector3.left * _strengthOfWind * Time.deltaTime);
    }
}
