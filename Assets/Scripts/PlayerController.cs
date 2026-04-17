using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private SpriteRenderer sr;

    [Header("PowerUps")]
    public float speedMultiplier = 1.5f;
    public float speedBoostDuration = 5f;
    public float jumpMultiplier = 1.35f;
    public float jumpBoostDuration = 5f;

    private float originalSpeed;
    private float originalJumpForce;
    private bool isSpeedBoosted = false;
    private bool isJumpBoosted = false;
    private bool isInvincible = false;
    private Coroutine speedBoostCoroutine;
    private Coroutine jumpBoostCoroutine;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();

        if (movement != null)
        {
            originalSpeed = movement.moveSpeed;
            originalJumpForce = movement.jumpForce;
        }
    }

    public void ActivateSpeedBoost(float boostDuration = -1f)
    {
        if (movement == null) return;

        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        float appliedDuration = boostDuration > 0f ? boostDuration : speedBoostDuration;
        speedBoostCoroutine = StartCoroutine(SpeedBoost(appliedDuration));
    }

    IEnumerator SpeedBoost(float boostDuration)
    {
        isSpeedBoosted = true;
        movement.SetSpeed(originalSpeed * speedMultiplier);

        yield return new WaitForSeconds(boostDuration);

        movement.SetSpeed(originalSpeed);
        isSpeedBoosted = false;
        speedBoostCoroutine = null;
    }

    public void ActivateJumpBoost(float boostDuration = -1f)
    {
        if (movement == null) return;

        if (jumpBoostCoroutine != null)
        {
            StopCoroutine(jumpBoostCoroutine);
        }

        float appliedDuration = boostDuration > 0f ? boostDuration : jumpBoostDuration;
        jumpBoostCoroutine = StartCoroutine(JumpBoost(appliedDuration));
    }

    IEnumerator JumpBoost(float boostDuration)
    {
        isJumpBoosted = true;
        movement.SetJumpForce(originalJumpForce * jumpMultiplier);

        yield return new WaitForSeconds(boostDuration);

        movement.SetJumpForce(originalJumpForce);
        isJumpBoosted = false;
        jumpBoostCoroutine = null;
    }

    public void ActivateInvincible()
    {
        if (!isInvincible)
            StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        isInvincible = true;

        for (int i = 0; i < 10; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.2f);
        }

        sr.enabled = true;
        isInvincible = false;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
