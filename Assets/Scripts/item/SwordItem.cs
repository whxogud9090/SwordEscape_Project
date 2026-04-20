using UnityEngine;

public class SwordItem : MonoBehaviour
{
    [Header("Final Sword")]
    [SerializeField] private bool isFinalSword;
    [SerializeField] private BatSwarmController batSwarm;
    [SerializeField] private float batDespawnDelay = 0.8f;

    private void Awake()
    {
        if (isFinalSword && batSwarm == null)
        {
            batSwarm = FindFirstObjectByType<BatSwarmController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.AddSword();
            }

            if (AudioBootstrap.Instance != null && !isFinalSword)
            {
                AudioBootstrap.Instance.PlaySwordPickup(false);
            }

            if (isFinalSword)
            {
                if (batSwarm == null)
                {
                    batSwarm = FindFirstObjectByType<BatSwarmController>();
                }

                if (batSwarm != null)
                {
                    batSwarm.StopAndDespawn(batDespawnDelay);
                }
                else
                {
                    Debug.LogWarning("SwordItem: Final sword could not find BatSwarmController.", this);
                }

                if (GameManager.instance != null)
                {
                    GameManager.instance.ShowClearMessage();
                }
            }

            Destroy(gameObject);
        }
    }
}
