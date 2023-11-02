using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadEndLine : MonoBehaviour, ILine
{
    public void LineAction(Player player)
    {
        player.PlayerAnimator.TrampolineEnd();
    }
}
