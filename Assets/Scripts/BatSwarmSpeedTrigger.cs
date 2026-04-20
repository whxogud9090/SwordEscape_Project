using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BatSwarmSpeedTrigger : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private BatSwarmController batSwarm;

    [Header("Trigger")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float boostedSpeed = 7f;
    [SerializeField] private bool oneShot = true;

    private bool hasTriggered;

    private void Reset()
    {
        EnsureTriggerCollider();
        AutoFindBatSwarm();
    }

    private void Awake()
    {
        EnsureTriggerCollider();
        AutoFindBatSwarm();
    }

    private void OnValidate()
    {
        EnsureTriggerCollider();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && oneShot)
        {
            return;
        }

        if (!other.CompareTag(playerTag))
        {
            return;
        }

        AutoFindBatSwarm();
        if (batSwarm == null)
        {
            Debug.LogWarning("BatSwarmSpeedTrigger: BatSwarmController reference is missing.", this);
            return;
        }

        batSwarm.SetMoveSpeed(boostedSpeed);
        hasTriggered = true;
    }

    private void EnsureTriggerCollider()
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    private void AutoFindBatSwarm()
    {
        if (batSwarm == null)
        {
            batSwarm = FindFirstObjectByType<BatSwarmController>();
        }
    }
}
