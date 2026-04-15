using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 데미지 받을 때 호출
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // 무적이면 무시

        // 데미지 처리 로직
        Debug.Log("데미지 받음: " + damage);
    }

    // 무적 상태 시작
    public void ActivateInvincibility(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;

        // 깜빡임 효과 (선택사항)
        float elapsed = 0f;
        while (elapsed < duration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}