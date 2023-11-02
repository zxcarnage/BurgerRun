using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCanvas : MonoBehaviour
{
    public static InGameCanvas Instance;
    
    public Canvas Canvas { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        Canvas = GetComponent<Canvas>();
    }


}
