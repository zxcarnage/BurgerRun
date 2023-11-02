using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelBeams : MonoBehaviour
{
    [SerializeField] private Transform _left;
    [SerializeField] private Transform _right;
    [SerializeField] private Transform _centerLeft;
    [SerializeField] private Transform _centerRight;

    public float LeftXPosition => Mathf.Lerp(_left.transform.position.x, _centerLeft.transform.position.x, 0.5f);
    public float CenterXPosition => Mathf.Lerp(_centerLeft.transform.position.x, _centerRight.transform.position.x, 0.5f);
    public float RightXPosition => Mathf.Lerp(_centerRight.transform.position.x, _right.transform.position.x, 0.5f);

    public float GetLaneXPosition(int index)
    {
        switch (index)
        {
            case 0:
                return LeftXPosition;
            case 1:
                return CenterXPosition;
            case 2:
                return RightXPosition;
            default:
                return CenterXPosition;
        }
    }

}
