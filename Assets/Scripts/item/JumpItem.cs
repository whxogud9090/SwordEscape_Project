using UnityEngine;

public class JumpItem : MonoBehaviour
{
    public float jumpBoostDuration = 5f;

    [Header("Item Data")]
    [SerializeField] private ItemSO itemData;
    [SerializeField] private int fallbackScore = 30;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.ActivateJumpBoost(jumpBoostDuration);
            AddItemScore();
            if (AudioBootstrap.Instance != null)
            {
                AudioBootstrap.Instance.PlayItemPickup();
            }
            Destroy(gameObject);
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
