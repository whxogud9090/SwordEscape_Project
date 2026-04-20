using UnityEngine;

public class JumpItem : MonoBehaviour
{
    public float jumpBoostDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.ActivateJumpBoost(jumpBoostDuration);
            if (AudioBootstrap.Instance != null)
            {
                AudioBootstrap.Instance.PlayItemPickup();
            }
            Destroy(gameObject);
        }
    }
}
