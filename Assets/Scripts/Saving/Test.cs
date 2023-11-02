using UnityEngine;

public class Test : MonoBehaviour
{
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            DataManager.Instance.DebugResetSavings();   
    }
}
