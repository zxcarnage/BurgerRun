using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Tip : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    
    private Player _player;
    private Canvas _canvas;
    
    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        _canvas = GetComponent<Canvas>();
        StartCoroutine(FollowPlayer(_player));
    }

    private IEnumerator FollowPlayer(Player player)
    {
        while (true)
        {
            var canvasPosition = _canvas.transform.position;
            var playerPosition = player.transform.position;
            _canvas.transform.position = playerPosition + _offset;
            yield return null;
        }
    }
}
