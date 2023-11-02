using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderWall : MonoBehaviour
{
    [SerializeField] private float _strength = 10;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("Stopped player");
            Vector3 velocity = player.PlayerMover.Rigidbody.velocity;
            //velocity.x = -velocity.x;
            Debug.Log(velocity);
            player.PlayerMover.Rigidbody.AddForce(new Vector3(_strength, 0, 0), ForceMode.Impulse);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("Stopping player");
            Vector3 velocity = player.PlayerMover.Rigidbody.velocity;
            velocity.x = _strength;
            //player.PlayerMover.Rigidbody.velocity = velocity;
            player.PlayerMover.Rigidbody.AddForce(new Vector3(_strength, 0, 0), ForceMode.Force);
            player.PlayerMover.TryMove();
        }
    }
}
