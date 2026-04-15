using UnityEngine;

public class JumpItem : MonoBehaviour
{
    public float jumpBoostDuration = 5f; // 지속 시간

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.ActivateJumpBoost(jumpBoostDuration);
                Destroy(gameObject);
            }
        }
    }
}