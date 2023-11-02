using UnityEngine;

[RequireComponent(typeof(Level))]
public class ShortLevel : MonoBehaviour, ILevel
{
    [SerializeField] private TwerkBattle _battle;
    public void SpawnEnding(Vector3 endingPosition)
    {
        var battle = Instantiate(_battle, endingPosition, _battle.transform.rotation);
        battle.transform.SetParent(transform);
        battle.Spawn(endingPosition);
    }
}
