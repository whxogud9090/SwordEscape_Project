using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 3f;

    private Vector3 startPos;
    private int direction = 1;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    void Awake()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 moveStep = Vector2.right * direction * moveSpeed * Time.fixedDeltaTime;

        if (rb != null && rb.bodyType != RigidbodyType2D.Static)
        {
            rb.MovePosition(rb.position + moveStep);
        }
        else
        {
            transform.Translate(moveStep, Space.World);
        }

        if (transform.position.x >= startPos.x + moveDistance)
        {
            direction = -1;
        }
        else if (transform.position.x <= startPos.x - moveDistance)
        {
            direction = 1;
        }

        if (sr != null && direction != 0)
        {
            sr.flipX = direction > 0;
        }
    }
}
