using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadmillRamp : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.PlayerMover.OnTreadmill = true;
        }
    }
}
