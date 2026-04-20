using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public float speedBoostDuration = 5f;

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
            if (AudioBootstrap.Instance != null)
            {
                AudioBootstrap.Instance.PlayItemPickup();
            }
            Destroy(gameObject);
        }
    }
}
