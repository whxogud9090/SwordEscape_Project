using UnityEngine;

public class SwordItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.AddSword();
            Destroy(gameObject);
        }
    }
}