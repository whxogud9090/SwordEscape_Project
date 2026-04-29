using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentSword = 0;
    public int requiredSword = 3;

    public TextMeshProUGUI swordText;
    public TextMeshProUGUI clearText;

    [Header("Clear UI")]
    [SerializeField] private string clearMessage = "CLEAR!";
    [SerializeField] private Vector2 clearTextPosition = new Vector2(0f, -80f);

    private float stageStartTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        stageStartTime = Time.time;
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

    public void ShowClearMessage()
    {
        EnsureClearText();

        if (clearText == null)
        {
            return;
        }

        clearText.text = clearMessage;
        clearText.gameObject.SetActive(true);

        if (AudioBootstrap.Instance != null)
        {
            AudioBootstrap.Instance.PlayClear();
        }
    }

    public int GetStageScore()
    {
        int timeBonus = Mathf.Max(0, 10000 - Mathf.RoundToInt((Time.time - stageStartTime) * 100f));
        int swordBonus = currentSword * 1000;
        return timeBonus + swordBonus;
    }

    private void EnsureClearText()
    {
        if (clearText != null)
        {
            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("RuntimeClearCanvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        GameObject textObject = new GameObject("ClearText");
        textObject.transform.SetParent(canvas.transform, false);

        clearText = textObject.AddComponent<TextMeshProUGUI>();
        clearText.text = clearMessage;
        clearText.alignment = TextAlignmentOptions.Center;
        clearText.fontSize = 42;
        clearText.color = new Color(1f, 0.92f, 0.55f);
        clearText.enableAutoSizing = false;
        clearText.outlineWidth = 0.2f;
        clearText.outlineColor = new Color(0.12f, 0.08f, 0.02f, 1f);
        clearText.gameObject.SetActive(false);

        RectTransform rectTransform = clearText.rectTransform;
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = clearTextPosition;
        rectTransform.sizeDelta = new Vector2(500f, 100f);
    }
}
