using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    
    public void HandleFoodCollision(Player player, IEatable food)
    { 
        food.Eat(player);
    }

    public void HandleObstacleCollision(Player player, IObstacle obstacle)
    {
        obstacle.AffectPlayer(player);
    }

    public void HandleLineTrigger(Player player, ILine line)
    {
        line.LineAction(player);
    }

    public void HandleEnemyTrigger(Player player, IEnemy enemy)
    {
        enemy.InterractWith(player);
    }

    public void HandleZoneTrigger(Player player, IZone zone)
    {
        zone.AffectCanvas(player);
    }

    public void HandleAreaTriggerEnter(Player player, IAreaTrigger area)
    {
        area.EnterArea(player);
    }

    public void HandleAreaTriggerStay(Player player, IAreaTrigger area)
    {
        area.StayInArea(player);
    }

    public void HandleAreaTriggerExit(Player player, IAreaTrigger area)
    {
        area.ExitArea(player);
    }
}
