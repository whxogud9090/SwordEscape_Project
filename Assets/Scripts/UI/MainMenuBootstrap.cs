using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBootstrap : MonoBehaviour
{
    [Header("Scene Flow")]
    [SerializeField] private string firstStageSceneName = "Stage1";

    [Header("Menu Copy")]
    [SerializeField] private string gameTitle = "SWORD ESCAPE";
    [SerializeField] private string subtitle = "Collect three swords and survive every trap.";

    private GameObject helpOverlay;
    private RectTransform titleCard;
    private Button startButton;

    private void Awake()
    {
        ConfigureCamera();
        EnsureEventSystem();
        BuildMenu();
    }

    private void Update()
    {
        if (titleCard == null)
        {
            return;
        }

        float offset = Mathf.Sin(Time.unscaledTime * 1.35f) * 8f;
        titleCard.anchoredPosition = new Vector2(0f, offset);
    }

    private void ConfigureCamera()
    {
        Camera cameraRef = Camera.main;
        if (cameraRef == null)
        {
            cameraRef = FindObjectOfType<Camera>();
        }

        if (cameraRef == null)
        {
            return;
        }

        cameraRef.clearFlags = CameraClearFlags.SolidColor;
        cameraRef.backgroundColor = new Color32(17, 27, 55, 255);
        cameraRef.orthographic = true;
        cameraRef.orthographicSize = 5f;
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<EventSystem>() != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<InputSystemUIInputModule>();
    }

    private void BuildMenu()
    {
        if (FindObjectOfType<Canvas>() != null)
        {
            return;
        }

        TMP_FontAsset fontAsset = TMP_Settings.defaultFontAsset;
        if (fontAsset == null)
        {
            fontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        }

        Canvas canvas = CreateCanvas();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        CreateFullScreenImage("Background", canvasRect, new Color32(9, 15, 33, 255));
        CreateSkyLayers(canvasRect);
        CreateGroundLayers(canvasRect);

        RectTransform contentRoot = CreateTransparentRect("ContentRoot", canvasRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

        titleCard = CreatePanel("TitleCard", contentRoot, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(120f, -92f), new Vector2(720f, 260f), new Color32(15, 20, 41, 205));
        AddOutline(titleCard.gameObject, new Color32(255, 188, 82, 130));
        CreateAccentBars(titleCard);

        CreateText("Title", titleCard, gameTitle, fontAsset, 92, FontStyles.Bold, new Color32(245, 246, 255, 255), new Vector2(38f, -74f), new Vector2(620f, 95f));
        CreateText("Subtitle", titleCard, subtitle, fontAsset, 30, FontStyles.Normal, new Color32(165, 199, 255, 255), new Vector2(42f, -146f), new Vector2(610f, 52f));
        CreateText("Hook", titleCard, "Run fast. Jump clean. Escape alive.", fontAsset, 25, FontStyles.Bold, new Color32(255, 205, 117, 255), new Vector2(42f, -196f), new Vector2(520f, 40f));

        RectTransform taglineBand = CreatePanel("TaglineBand", contentRoot, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(122f, -374f), new Vector2(760f, 64f), new Color32(13, 19, 37, 220));
        AddOutline(taglineBand.gameObject, new Color32(92, 214, 196, 80));
        CreateText("Tagline", taglineBand, "Collect 3 swords, avoid traps, and unlock the exit.", fontAsset, 24, FontStyles.Normal, new Color32(231, 237, 255, 255), new Vector2(24f, -18f), new Vector2(680f, 28f));

        RectTransform buttonRoot = CreateTransparentRect("ButtonRoot", contentRoot, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(122f, -470f), new Vector2(460f, 320f));
        startButton = CreateMenuButton(buttonRoot, "START GAME", new Vector2(0f, 0f), StartGame, fontAsset, width: 420f, height: 84f, primary: true);
        CreateMenuButton(buttonRoot, "HELP", new Vector2(0f, -102f), OpenHelp, fontAsset, width: 420f, height: 72f);
        CreateMenuButton(buttonRoot, "EXIT", new Vector2(0f, -192f), QuitGame, fontAsset, width: 420f, height: 72f);

        RectTransform infoCard = CreatePanel("InfoCard", contentRoot, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-116f, -98f), new Vector2(460f, 250f), new Color32(15, 22, 45, 228));
        AddOutline(infoCard.gameObject, new Color32(90, 214, 196, 120));
        CreateText("InfoHeader", infoCard, "MISSION", fontAsset, 34, FontStyles.Bold, new Color32(90, 214, 196, 255), new Vector2(26f, -32f), new Vector2(240f, 38f));
        CreateText("InfoBody", infoCard, "Find 3 swords\nSurvive spikes and enemies\nReach the door to clear the stage", fontAsset, 28, FontStyles.Normal, new Color32(239, 243, 255, 255), new Vector2(26f, -88f), new Vector2(350f, 130f));
        CreateText("InfoFooter", infoCard, "Best played with keyboard controls", fontAsset, 20, FontStyles.Normal, new Color32(160, 173, 204, 255), new Vector2(26f, -206f), new Vector2(320f, 28f));

        RectTransform previewCard = CreatePanel("PreviewCard", contentRoot, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-116f, 178f), new Vector2(520f, 300f), new Color32(12, 18, 37, 230));
        AddOutline(previewCard.gameObject, new Color32(255, 188, 82, 105));
        CreateText("PreviewTitle", previewCard, "STAGE FLOW", fontAsset, 30, FontStyles.Bold, new Color32(255, 205, 117, 255), new Vector2(24f, -26f), new Vector2(240f, 34f));
        CreatePreviewTrack(previewCard);
        CreateText("PreviewHint", previewCard, "Boost items open safer routes and help you outrun danger.", fontAsset, 20, FontStyles.Normal, new Color32(174, 188, 221, 255), new Vector2(24f, -248f), new Vector2(430f, 36f));

        CreateText("Footer", contentRoot, "Press START GAME to jump straight into Stage 1.", fontAsset, 24, FontStyles.Normal, new Color32(209, 216, 236, 255), new Vector2(124f, -910f), new Vector2(620f, 36f), TextAlignmentOptions.Left);

        helpOverlay = CreateHelpOverlay(canvasRect, fontAsset);
        helpOverlay.SetActive(false);

        EventSystem currentEventSystem = EventSystem.current;
        if (currentEventSystem != null)
        {
            currentEventSystem.SetSelectedGameObject(startButton.gameObject);
        }
    }

    private void CreateSkyLayers(RectTransform parent)
    {
        RectTransform glow = CreateTransparentRect("SkyGlow", parent, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -120f), new Vector2(1500f, 360f));
        Image glowImage = glow.gameObject.AddComponent<Image>();
        glowImage.color = new Color32(39, 71, 130, 110);

        RectTransform moon = CreateTransparentRect("Moon", parent, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-220f, -120f), new Vector2(150f, 150f));
        Image moonImage = moon.gameObject.AddComponent<Image>();
        moonImage.color = new Color32(240, 244, 255, 210);

        CreateDecorativeDot(parent, new Vector2(-360f, -160f), 14f, new Color32(255, 205, 117, 255));
        CreateDecorativeDot(parent, new Vector2(-270f, -230f), 10f, new Color32(90, 214, 196, 255));
        CreateDecorativeDot(parent, new Vector2(-170f, -195f), 8f, new Color32(174, 196, 255, 255));
        CreateDecorativeDot(parent, new Vector2(-120f, -285f), 12f, new Color32(255, 205, 117, 220));
    }

    private void CreateGroundLayers(RectTransform parent)
    {
        CreateBottomBand(parent);

        RectTransform farHill = CreateTransparentRect("FarHill", parent, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 180f), new Vector2(1920f, 240f));
        Image farHillImage = farHill.gameObject.AddComponent<Image>();
        farHillImage.color = new Color32(18, 31, 58, 255);

        RectTransform nearHill = CreateTransparentRect("NearHill", parent, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 100f), new Vector2(1920f, 180f));
        Image nearHillImage = nearHill.gameObject.AddComponent<Image>();
        nearHillImage.color = new Color32(12, 22, 42, 255);

        RectTransform groundTop = CreateTransparentRect("GroundTop", parent, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 84f), new Vector2(1920f, 26f));
        Image groundTopImage = groundTop.gameObject.AddComponent<Image>();
        groundTopImage.color = new Color32(95, 164, 78, 255);

        RectTransform ground = CreateTransparentRect("Ground", parent, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), Vector2.zero, new Vector2(1920f, 88f));
        Image groundImage = ground.gameObject.AddComponent<Image>();
        groundImage.color = new Color32(33, 26, 59, 255);
    }

    private Canvas CreateCanvas()
    {
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();

        RectTransform rect = canvas.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        return canvas;
    }

    private void CreateTopGlow(RectTransform parent)
    {
        RectTransform glow = CreateTransparentRect("TopGlow", parent, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, 0f), new Vector2(1920f, 200f));
        Image glowImage = glow.gameObject.AddComponent<Image>();
        glowImage.color = new Color32(44, 83, 145, 90);
    }

    private void CreateBottomBand(RectTransform parent)
    {
        RectTransform bottomBand = CreateTransparentRect("BottomBand", parent, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), Vector2.zero, new Vector2(1920f, 92f));
        Image bandImage = bottomBand.gameObject.AddComponent<Image>();
        bandImage.color = new Color32(9, 14, 29, 235);
    }

    private void CreateAccentBars(RectTransform parent)
    {
        CreateDecorativeBar(parent, new Vector2(44f, -28f), new Vector2(120f, 10f), new Color32(77, 233, 194, 255));
        CreateDecorativeBar(parent, new Vector2(168f, -28f), new Vector2(80f, 10f), new Color32(255, 143, 77, 255));
    }

    private void CreateDecorativeBar(RectTransform parent, Vector2 anchoredPosition, Vector2 size, Color color)
    {
        RectTransform bar = CreateTransparentRect("AccentBar", parent, new Vector2(0f, 1f), new Vector2(0f, 1f), anchoredPosition, size);
        Image image = bar.gameObject.AddComponent<Image>();
        image.color = color;
    }

    private void CreateDecorativeDot(RectTransform parent, Vector2 anchoredPosition, float size, Color color)
    {
        RectTransform dot = CreateTransparentRect("Dot", parent, new Vector2(1f, 1f), new Vector2(1f, 1f), anchoredPosition, new Vector2(size, size));
        Image image = dot.gameObject.AddComponent<Image>();
        image.color = color;
    }

    private void CreatePreviewTrack(RectTransform parent)
    {
        RectTransform track = CreateTransparentRect("Track", parent, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(24f, -74f), new Vector2(420f, 126f));
        Image trackImage = track.gameObject.AddComponent<Image>();
        trackImage.color = new Color32(30, 43, 78, 255);

        CreateTrackBlock(track, new Vector2(18f, -82f), new Vector2(72f, 24f), new Color32(95, 164, 78, 255));
        CreateTrackBlock(track, new Vector2(112f, -66f), new Vector2(58f, 24f), new Color32(95, 164, 78, 255));
        CreateTrackBlock(track, new Vector2(196f, -92f), new Vector2(52f, 24f), new Color32(95, 164, 78, 255));
        CreateTrackBlock(track, new Vector2(282f, -58f), new Vector2(66f, 24f), new Color32(95, 164, 78, 255));
        CreateTrackBlock(track, new Vector2(376f, -82f), new Vector2(30f, 24f), new Color32(95, 164, 78, 255));

        CreateTrackBlock(track, new Vector2(82f, -98f), new Vector2(18f, 18f), new Color32(233, 237, 246, 255));
        CreateTrackBlock(track, new Vector2(256f, -112f), new Vector2(18f, 18f), new Color32(255, 205, 117, 255));
        CreateTrackBlock(track, new Vector2(340f, -36f), new Vector2(18f, 18f), new Color32(90, 214, 196, 255));
    }

    private void CreateTrackBlock(RectTransform parent, Vector2 anchoredPosition, Vector2 size, Color color)
    {
        RectTransform block = CreateTransparentRect("TrackBlock", parent, new Vector2(0f, 1f), new Vector2(0f, 1f), anchoredPosition, size);
        Image image = block.gameObject.AddComponent<Image>();
        image.color = color;
    }

    private GameObject CreateHelpOverlay(RectTransform parent, TMP_FontAsset fontAsset)
    {
        RectTransform overlay = CreateTransparentRect("HelpOverlay", parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(1920f, 1080f));
        Image overlayImage = overlay.gameObject.AddComponent<Image>();
        overlayImage.color = new Color32(4, 8, 18, 200);

        RectTransform popup = CreatePanel("HelpPopup", overlay, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(920f, 560f), new Color32(18, 27, 52, 245));
        AddOutline(popup.gameObject, new Color32(255, 204, 109, 140));

        CreateText("HelpTitle", popup, "HOW TO PLAY", fontAsset, 52, FontStyles.Bold, new Color32(244, 247, 255, 255), new Vector2(50f, -56f), new Vector2(500f, 60f));
        CreateText("HelpBody", popup,
            "Move : A / D or Left / Right Arrow\nJump : Space\n\nGoal : Collect all swords and reach the exit.\n\nTips\n- Speed items help you escape chase sections.\n- Jump items open safer routes.\n- Invincibility items let you cross dangerous zones.",
            fontAsset, 28, FontStyles.Normal, new Color32(225, 231, 247, 255), new Vector2(50f, -140f), new Vector2(780f, 280f));

        CreateText("HelpFooter", popup, "Press HELP again or CLOSE to return to the menu.", fontAsset, 22, FontStyles.Normal, new Color32(153, 167, 204, 255), new Vector2(50f, -458f), new Vector2(600f, 30f));

        CreateMenuButton(popup, "CLOSE", new Vector2(0f, -210f), CloseHelp, fontAsset, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));

        return overlay.gameObject;
    }

    private RectTransform CreatePanel(string objectName, RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 size, Color color)
    {
        RectTransform rect = CreateTransparentRect(objectName, parent, anchorMin, anchorMax, anchoredPosition, size);
        Image image = rect.gameObject.AddComponent<Image>();
        image.color = color;
        return rect;
    }

    private RectTransform CreateTransparentRect(string objectName, RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 size)
    {
        GameObject objectRef = new GameObject(objectName, typeof(RectTransform));
        RectTransform rect = objectRef.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = anchorMin == anchorMax ? anchorMin : new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;
        return rect;
    }

    private TextMeshProUGUI CreateText(
        string objectName,
        RectTransform parent,
        string content,
        TMP_FontAsset fontAsset,
        float fontSize,
        FontStyles fontStyle,
        Color color,
        Vector2 anchoredPosition,
        Vector2 size,
        TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft)
    {
        GameObject textObject = new GameObject(objectName, typeof(RectTransform));
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.font = fontAsset;
        text.text = content;
        text.fontSize = fontSize;
        text.fontStyle = fontStyle;
        text.color = color;
        text.alignment = alignment;

        Shadow shadow = textObject.AddComponent<Shadow>();
        shadow.effectColor = new Color32(0, 0, 0, 110);
        shadow.effectDistance = new Vector2(2f, -2f);

        return text;
    }

    private Button CreateMenuButton(
        RectTransform parent,
        string label,
        Vector2 anchoredPosition,
        UnityEngine.Events.UnityAction onClick,
        TMP_FontAsset fontAsset,
        Vector2? anchorMin = null,
        Vector2? anchorMax = null,
        float width = 360f,
        float height = 72f,
        bool primary = false)
    {
        GameObject buttonObject = new GameObject(label, typeof(RectTransform));
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = anchorMin ?? new Vector2(0f, 1f);
        rect.anchorMax = anchorMax ?? new Vector2(0f, 1f);
        rect.pivot = rect.anchorMin == rect.anchorMax ? rect.anchorMin : new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(width, height);

        Image image = buttonObject.AddComponent<Image>();
        image.color = primary ? new Color32(255, 150, 78, 255) : new Color32(29, 52, 108, 255);

        Outline outline = buttonObject.AddComponent<Outline>();
        outline.effectColor = primary ? new Color32(255, 223, 151, 120) : new Color32(133, 214, 255, 120);
        outline.effectDistance = new Vector2(3f, -3f);

        Button button = buttonObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = primary ? new Color32(255, 150, 78, 255) : new Color32(29, 52, 108, 255);
        colors.highlightedColor = primary ? new Color32(255, 182, 102, 255) : new Color32(49, 82, 157, 255);
        colors.selectedColor = primary ? new Color32(255, 182, 102, 255) : new Color32(49, 82, 157, 255);
        colors.pressedColor = primary ? new Color32(229, 124, 54, 255) : new Color32(18, 36, 79, 255);
        colors.disabledColor = new Color32(60, 60, 60, 180);
        colors.fadeDuration = 0.05f;
        button.colors = colors;
        button.targetGraphic = image;
        button.onClick.AddListener(onClick);

        TextMeshProUGUI labelText = CreateText("Label", rect, label, fontAsset, primary ? 32 : 28, FontStyles.Bold, Color.white, new Vector2(0f, -10f), new Vector2(width, 52f), TextAlignmentOptions.Center);
        RectTransform labelRect = labelText.rectTransform;
        labelRect.anchorMin = new Vector2(0f, 0f);
        labelRect.anchorMax = new Vector2(1f, 1f);
        labelRect.pivot = new Vector2(0.5f, 0.5f);
        labelRect.anchoredPosition = Vector2.zero;
        labelRect.sizeDelta = Vector2.zero;

        return button;
    }

    private void AddOutline(GameObject target, Color color)
    {
        Outline outline = target.AddComponent<Outline>();
        outline.effectColor = color;
        outline.effectDistance = new Vector2(3f, -3f);
    }

    private void CreateFullScreenImage(string objectName, RectTransform parent, Color color)
    {
        RectTransform rect = CreateTransparentRect(objectName, parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = rect.gameObject.AddComponent<Image>();
        image.color = color;
    }

    private void StartGame()
    {
        SceneManager.LoadScene(firstStageSceneName);
    }

    private void OpenHelp()
    {
        if (helpOverlay != null)
        {
            helpOverlay.SetActive(true);
        }
    }

    private void CloseHelp()
    {
        if (helpOverlay != null)
        {
            helpOverlay.SetActive(false);
            if (EventSystem.current != null && startButton != null)
            {
                EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            }
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
