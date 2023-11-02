using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAreaTrigger
{
    public void EnterArea(Player player);
    public void StayInArea(Player player);
    public void ExitArea(Player player);
}
