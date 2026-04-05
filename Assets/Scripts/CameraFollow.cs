using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offsetX = 0f;
    public float fixedY = 1f;

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(
                target.position.x + offsetX,
                fixedY,
                transform.position.z
            );
        }
    }
}