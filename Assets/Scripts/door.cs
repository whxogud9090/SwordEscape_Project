using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance.currentSword >= GameManager.instance.requiredSword)
            {
                Debug.Log("클리어!");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log("검이 부족합니다!");
            }
        }
    }
}