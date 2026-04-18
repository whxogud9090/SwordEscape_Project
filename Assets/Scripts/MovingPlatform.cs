using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 localMoveOffset = new Vector2(6f, 0f);
    public float moveSpeed = 1.8f;
    public bool startFromOffset = false;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Rigidbody2D rb;
    private float journeyLength;
    private Transform pendingParent;
    private Transform pendingUnparent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        endPosition = startPosition + (Vector3)localMoveOffset;

        if (startFromOffset)
        {
            transform.position = endPosition;
        }

        journeyLength = Vector3.Distance(startPosition, endPosition);
    }

    private void FixedUpdate()
    {
        if (journeyLength <= 0.01f)
        {
            return;
        }

        float t = Mathf.PingPong(Time.time * moveSpeed / journeyLength, 1f);
        Vector3 nextPosition = Vector3.Lerp(startPosition, endPosition, t);

        if (rb != null)
        {
            rb.MovePosition(nextPosition);
            return;
        }

        transform.position = nextPosition;
    }

    private void LateUpdate()
    {
        if (pendingParent != null)
        {
            if (isActiveAndEnabled && gameObject.activeInHierarchy && pendingParent.gameObject.activeInHierarchy)
            {
                pendingParent.SetParent(transform);
            }

            pendingParent = null;
        }

        if (pendingUnparent != null)
        {
            if (pendingUnparent.parent == transform)
            {
                pendingUnparent.SetParent(null);
            }

            pendingUnparent = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            pendingParent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && collision.transform.parent == transform)
        {
            pendingUnparent = collision.transform;
        }
    }
}
