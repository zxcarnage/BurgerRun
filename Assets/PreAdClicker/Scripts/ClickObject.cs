using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    
    // Если игру на паузу ставить будешь - поменяй на update, да будет от фпс зависить, но fixed вообще работать не будет
    private void FixedUpdate()
    {
        transform.position += Vector3.up * (moveSpeed * Time.fixedDeltaTime);
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
