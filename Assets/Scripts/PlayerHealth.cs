using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || isDead) return;

        Debug.Log("Player took damage: " + damage);
    }

    public void Die()
    {
        if (isInvincible || isDead) return;

        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        isDead = true;

        if (AudioBootstrap.Instance != null)
        {
            AudioBootstrap.Instance.PlayDeath();
        }

        if (playerMovement != null)
        {
            playerMovement.SetMove(false);
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        yield return new WaitForSeconds(0.2f);

        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    public void ActivateInvincibility(float duration)
    {
        if (isDead) return;
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Die();
        }
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isInvincible = false;
    }
}
