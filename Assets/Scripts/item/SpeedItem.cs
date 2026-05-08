using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public float speedBoostDuration = 5f;

    [Header("Item Data")]
    [SerializeField] private ItemSO itemData;
    [SerializeField] private int fallbackScore = 30;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
        {
            player = other.GetComponentInParent<PlayerController>();
        }

        if (player == null && other.attachedRigidbody != null)
        {
            player = other.attachedRigidbody.GetComponent<PlayerController>();
        }

        if (player != null)
        {
            player.ActivateSpeedBoost(speedBoostDuration);
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
