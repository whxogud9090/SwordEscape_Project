using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 3f;

    private Vector3 startPos;
    private int direction = 1;
    private SpriteRenderer sr;

    void Start()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 이동
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // 방향 전환 위치 체크
        if (transform.position.x >= startPos.x + moveDistance)
        {
            direction = -1;
        }
        else if (transform.position.x <= startPos.x - moveDistance)
        {
            direction = 1;
        }

        // ⭐ 방향에 따라 스프라이트 뒤집기
        if (direction != 0)
        {
            sr.flipX = direction > 0;
        }
    }
}