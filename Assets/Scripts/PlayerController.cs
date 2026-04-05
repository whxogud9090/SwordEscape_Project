using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    private Rigidbody2D rb;
    private bool isGrounded;

    private Vector3 startPosition;
    private bool isInvincible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (moveX > 0)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // °¡½Ã ´êÀ¸¸é Á×±â (¹«Àû ¾Æ´Ò ¶§¸¸)
        if (other.CompareTag("Spike"))
        {
            if (!isInvincible)
            {
                Die();
            }
        }

        // ¹«Àû ¾ÆÀÌÅÛ ¸Ô±â
        if (other.CompareTag("Invincible"))
        {
            Destroy(other.gameObject);
            StartCoroutine(InvincibleTime());
        }
        if (other.CompareTag("Door"))
        {
            if (GameManager.instance.currentSword >= GameManager.instance.requiredSword)
            {
                SceneManager.LoadScene("Stage2");
            }
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
}