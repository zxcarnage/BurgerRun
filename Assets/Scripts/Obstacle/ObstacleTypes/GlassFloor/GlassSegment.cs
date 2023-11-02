using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GlassSegment : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationOffset;

    private List<Transform> childrenTransform;
    public void Break()
    {
        childrenTransform = new List<Transform>();
        GetComponentsInChildren(childrenTransform);
        childrenTransform.Remove(transform);
        Random random = new Random();
        foreach (Transform childTransform in childrenTransform)
        {
            if (childTransform.name == "GlssSide.002" || childTransform.name == "GlssSide.001")
                continue;
            float randomFloat = (float) random.NextDouble();
            var locarRotationOffset = new Vector3(0, _rotationOffset.y + randomFloat, _rotationOffset.z - randomFloat);
            childTransform.rotation = Quaternion.Euler(locarRotationOffset + childTransform.rotation.eulerAngles);
        }
    }
}
