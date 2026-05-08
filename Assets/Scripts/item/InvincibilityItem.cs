using UnityEngine;

public class InvincibilityItem : MonoBehaviour
{
    public float invincibilityDuration = 5f;

    [Header("Item Data")]
    [SerializeField] private ItemSO itemData;
    [SerializeField] private int fallbackScore = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.ActivateInvincibility(invincibilityDuration);
                AddItemScore();
                if (AudioBootstrap.Instance != null)
                {
                    AudioBootstrap.Instance.PlayItemPickup();
                }
                Destroy(gameObject);
            }
        }
    }

    private void AddItemScore()
    {
        if (GameManager.instance == null)
        {
            return;
        }

        int itemScore = itemData != null ? itemData.score : fallbackScore;
        GameManager.instance.AddScore(itemScore);
    }
}
