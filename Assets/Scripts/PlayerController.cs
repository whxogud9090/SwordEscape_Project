using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 15f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isInvincible = false;

    float originalSpeed;
    bool isSpeedUp = false;
    float moveX;

    Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed; // ⭐ 원래 속도 저장
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal");

        GetComponent<SpriteRenderer>().flipX = moveX < 0;
        if (moveX != 0)
        {
            GetComponent<SpriteRenderer>().flipX = moveX < 0;
        }
        else if (moveX < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        Jump();
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");

        Vector2 targetVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.2f);

        if (moveX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            Debug.Log("Get Sword!");
            Destroy(other.gameObject);
            GameManager.instance.currentSword++;
        }

       
        if (other.CompareTag("Spike") && !isInvincible)
        {
            Die();
        }

        
        if (other.CompareTag("Invincible"))
        {
            Destroy(other.gameObject);
            StartCoroutine(InvincibleTime());
        }

        if (other.CompareTag("Speed"))
        {
            Destroy(other.gameObject);
            StartCoroutine(SpeedUp());
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator InvincibleTime()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3f);
        isInvincible = false;
    }
    IEnumerator SpeedUp()
    {
        isSpeedUp = true;
        moveSpeed = originalSpeed * 2f; // 속도 2배

        yield return new WaitForSeconds(5f); // 5초 유지

        moveSpeed = originalSpeed;
        isSpeedUp = false;
    }
}