using UnityEngine;

public class BatSwarmController : MonoBehaviour
{
    [Header("Movement")]
    [Min(0f)]
    public float moveSpeed = 4f;
    public Vector2 moveDirection = Vector2.right;
    public bool useWorldSpace = true;
    public bool moveOnStart = true;

    [Header("Lifetime")]
    public bool destroyAfterDistance = false;
    [Min(0f)]
    public float maxTravelDistance = 60f;

    private Vector3 startPosition;
    private bool isMoving;
    private Coroutine despawnRoutine;

    private void Reset()
    {
        moveSpeed = 4f;
        moveDirection = Vector2.right;
        useWorldSpace = true;
        moveOnStart = true;
        destroyAfterDistance = false;
        maxTravelDistance = 60f;
    }

    private void Awake()
    {
        startPosition = transform.position;
        isMoving = moveOnStart;
    }

    private void OnEnable()
    {
        startPosition = useWorldSpace ? transform.position : transform.localPosition;
    }

    private void Update()
    {
        if (!isMoving || moveDirection.sqrMagnitude <= 0.0001f || moveSpeed <= 0f)
        {
            return;
        }

        Vector3 delta = (Vector3)(moveDirection.normalized * moveSpeed * Time.deltaTime);

        if (useWorldSpace)
        {
            transform.position += delta;
        }
        else
        {
            transform.localPosition += delta;
        }

        if (destroyAfterDistance)
        {
            Vector3 currentPosition = useWorldSpace ? transform.position : transform.localPosition;
            float traveled = Vector3.Distance(startPosition, currentPosition);
            if (traveled >= maxTravelDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = Mathf.Max(0f, newSpeed);
    }

    public void StopAndDespawn(float delay)
    {
        StopMoving();

        if (despawnRoutine != null)
        {
            StopCoroutine(despawnRoutine);
        }

        despawnRoutine = StartCoroutine(DespawnAfterDelay(Mathf.Max(0f, delay)));
    }

    private System.Collections.IEnumerator DespawnAfterDelay(float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        Destroy(gameObject);
    }
}
