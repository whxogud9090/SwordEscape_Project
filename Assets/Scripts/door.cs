using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string nextSceneName;

    [SerializeField] private float sceneLoadDelay = 0.22f;

    private bool isTransitioning;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTransitioning || !collision.CompareTag("Player"))
        {
            return;
        }

        if (GameManager.instance != null && GameManager.instance.currentSword >= GameManager.instance.requiredSword)
        {
            Debug.Log("Stage clear!");
            StartCoroutine(LoadNextSceneCoroutine(collision));
        }
        else
        {
            Debug.Log("Not enough swords!");
        }
    }

    private IEnumerator LoadNextSceneCoroutine(Collider2D collision)
    {
        isTransitioning = true;

        PlayerMovement playerMovement = collision.GetComponentInParent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetMove(false);
        }

        Rigidbody2D playerRb = collision.attachedRigidbody;
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
        }

        if (AudioBootstrap.Instance != null)
        {
            AudioBootstrap.Instance.PlayDoorOpen();
        }

        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(nextSceneName);
    }
}
