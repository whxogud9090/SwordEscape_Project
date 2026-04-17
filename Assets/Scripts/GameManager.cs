using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentSword = 0;
    public int requiredSword = 3;

    public TextMeshProUGUI swordText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddSword()
    {
        currentSword++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (swordText == null) return;
        swordText.text = currentSword + " / " + requiredSword;
    }
}
