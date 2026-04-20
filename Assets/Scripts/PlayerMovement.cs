using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D bodyCollider;
    private readonly RaycastHit2D[] groundHits = new RaycastHit2D[8];

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private float moveInput;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool isJumping;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bodyCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!canMove) return;

        moveInput = Input.GetAxis("Horizontal");

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (isGrounded && !isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
            isGrounded = false;
            if (AudioBootstrap.Instance != null)
            {
                AudioBootstrap.Instance.PlayJump();
            }
        }

        if (isGrounded && rb.linearVelocity.y <= 0.05f)
        {
            isJumping = false;
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveInput));
            anim.SetBool("IsJumping", !isGrounded);
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        isGrounded = CheckGrounded();
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetJumpForce(float force)
    {
        jumpForce = force;
    }

    public void SetMove(bool value)
    {
        canMove = value;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }

    private bool CheckGrounded()
    {
        // Ignore ground checks while the player is still rising from a jump.
        if (rb != null && rb.linearVelocity.y > 0.05f)
        {
            return false;
        }

        bool groundedByCheck = false;

        if (groundCheck != null)
        {
            groundedByCheck = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
            if (groundedByCheck)
            {
                return true;
            }
        }

        if (bodyCollider == null)
        {
            return false;
        }

        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = groundLayer;
        filter.useTriggers = false;

        int hitCount = bodyCollider.Cast(Vector2.down, filter, groundHits, checkRadius + 0.05f);
        return hitCount > 0;
    }
}
