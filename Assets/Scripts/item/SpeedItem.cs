using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public float speedBoostDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.ActivateSpeedBoost(speedBoostDuration);
            Destroy(gameObject);
        }
    }
}
