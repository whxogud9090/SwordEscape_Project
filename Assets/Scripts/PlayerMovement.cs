using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;

    public float jumpPower = 5f; // 점프력 조절
    bool isGrounded = true;      // 바닥에 닿아있는지 확인

    private float originalJumpPower;

    void Awake()
    {
        // 컴포넌트 불러오기
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 스페이스바를 누르고 & 바닥에 있을 때만 점프
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // 위쪽으로 힘을 가해 점프
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            // 애니메이터의 isJumping을 true로 바꿔서 점프 모션 실행
            anim.SetBool("isJumping", true);
            isGrounded = false;
        }
    }

    // 바닥에 다시 닿았을 때 점프 모션 해제
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 닿은 물체의 태그가 "Ground"라면 (맵 타일/바닥)
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // 애니메이터의 isJumping을 false로 바꿔서 기본 모션으로 복귀
            anim.SetBool("isJumping", false);
        }
    }
    public void ActivateJumpBoost(float duration)
    {
        StartCoroutine(JumpBoostCoroutine(duration));
    }

    private IEnumerator JumpBoostCoroutine(float duration)
    {
        originalJumpPower = jumpPower;  // 원래 점프력 저장
        jumpPower = jumpPower * 2f;     // 점프력 2배로 증가

        yield return new WaitForSeconds(duration);

        jumpPower = originalJumpPower;  // 원래 점프력으로 복구
    }
}