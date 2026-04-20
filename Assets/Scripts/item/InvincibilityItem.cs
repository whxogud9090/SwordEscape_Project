using UnityEngine;

public class InvincibilityItem : MonoBehaviour
{
    public float invincibilityDuration = 5f; // 무적 지속 시간 (초)

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 닿았을 때
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.ActivateInvincibility(invincibilityDuration);
                if (AudioBootstrap.Instance != null)
                {
                    AudioBootstrap.Instance.PlayItemPickup();
                }
                Destroy(gameObject); // 아이템 제거
            }
        }
    }
}
