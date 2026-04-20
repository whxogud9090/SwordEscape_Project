using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.12f;
    public Vector3 offset;
    public bool snapToPixelGrid = false;
    public float pixelsPerUnit = 16f;

    private Vector3 currentVelocity;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPos = target.position + offset;
        Vector3 nextPosition = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref currentVelocity,
            smoothTime);

        nextPosition.z = -10f;

        if (snapToPixelGrid && pixelsPerUnit > 0f)
        {
            float unitsPerPixel = 1f / pixelsPerUnit;
            nextPosition.x = Mathf.Round(nextPosition.x / unitsPerPixel) * unitsPerPixel;
            nextPosition.y = Mathf.Round(nextPosition.y / unitsPerPixel) * unitsPerPixel;
        }

        transform.position = nextPosition;
    }
}
