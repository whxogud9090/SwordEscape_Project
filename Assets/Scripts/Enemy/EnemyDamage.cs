using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // 무적이면 아무것도 안 함
                if (playerHealth.isInvincible) return;

                // 무적 아닐 때만 죽음 처리
                collision.SendMessage("Die");
            }
        }
    }
}