using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public void FollowPlayer(Vector3 playerPosition, Vector3 offset, Vector3 rotation)
    {
        var targetPosition = playerPosition + offset;
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
