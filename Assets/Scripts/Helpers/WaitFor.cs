using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitFor
{
    // Method for waiting a fixed amount of frames in a coroutine
    public static IEnumerator Frames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}
