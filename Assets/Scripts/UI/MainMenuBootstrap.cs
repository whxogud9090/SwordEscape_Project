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

    [Header("Artwork")]
    [SerializeField] private string backgroundResourcePath = "MainMenu/moonlit-forest-gate-bg";
    [SerializeField] private string koreanFontResourcePath = "Fonts/MonaS12TextKR";

    [Header("Menu Copy")]
    [SerializeField] private string gameTitle = "SWORD ESCAPE";
    [SerializeField] private string startLabel = "\uAC8C\uC784 \uC2DC\uC791";
    [SerializeField] private string helpLabel = "\uB3C4\uC6C0\uB9D0";
    [SerializeField] private string quitLabel = "\uAC8C\uC784 \uB098\uAC00\uAE30";
    [SerializeField] private string helpTitle = "\uB3C4\uC6C0\uB9D0";

    private GameObject helpOverlay;
    private Button startButton;
    private TMP_FontAsset latinFontAsset;
    private TMP_FontAsset koreanFontAsset;

    private void Awake()
    {
        ConfigureCamera();
        EnsureEventSystem();
        BuildMenu();
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
        cameraRef.backgroundColor = new Color32(14, 24, 49, 255);
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

        latinFontAsset = TMP_Settings.defaultFontAsset;
        if (latinFontAsset == null)
        {
            latinFontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        }

        koreanFontAsset = CreateDynamicFontAsset(koreanFontResourcePath);

        Canvas canvas = CreateCanvas();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Sprite backgroundSprite = LoadSpriteResource(backgroundResourcePath);
        if (backgroundSprite != null)
        {
            CreateFullScreenSprite("Background", canvasRect, backgroundSprite, Color.white);
        }
        else
        {
            CreateFullScreenImage("BackgroundFallback", canvasRect, new Color32(12, 24, 44, 255));
        }

        CreateFullScreenImage("Vignette", canvasRect, new Color32(7, 11, 23, 70));
        CreateBottomGradient(canvasRect);

        RectTransform topRightNote = CreatePanel(
            "TopRightNote",
            canvasRect,
            new Vector2(1f, 1f),
            new Vector2(1f, 1f),
            new Vector2(-48f, -34f),
            new Vector2(470f, 46f),
            new Color32(13, 24, 40, 118));
        CreateText(
            "BlogNote",
            topRightNote,
            "\uAC80\uC744 \uBAA8\uC544 \uD0C8\uCD9C\uD558\uB294 \uAE30\uC0AC \uD310\uD0C0\uC9C0 \uD50C\uB7AB\uD3EC\uBA38",
            GetFontForText("\uAC80\uC744 \uBAA8\uC544 \uD0C8\uCD9C\uD558\uB294 \uAE30\uC0AC \uD310\uD0C0\uC9C0 \uD50C\uB7AB\uD3EC\uBA38"),
            21,
            FontStyles.Bold,
            new Color32(239, 245, 255, 255),
            new Vector2(0f, -8f),
            new Vector2(440f, 30f),
            TextAlignmentOptions.Center);

        RectTransform titleRoot = CreateTransparentRect(
            "TitleRoot",
            canvasRect,
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -156f),
            new Vector2(1040f, 260f));
        CreateText(
            "TitleShadow",
            titleRoot,
            gameTitle,
            GetFontForText(gameTitle),
            114,
            FontStyles.Bold,
            new Color32(18, 34, 58, 175),
            new Vector2(7f, -7f),
            new Vector2(1040f, 124f),
            TextAlignmentOptions.Center);
        CreateText(
            "Title",
            titleRoot,
            gameTitle,
            GetFontForText(gameTitle),
            114,
            FontStyles.Bold,
            new Color32(246, 250, 255, 255),
            new Vector2(0f, 0f),
            new Vector2(1040f, 124f),
            TextAlignmentOptions.Center);
        CreateText(
            "Subtitle",
            titleRoot,
            "Moonlit escape beyond the ancient gate",
            GetFontForText("Moonlit escape beyond the ancient gate"),
            28,
            FontStyles.Bold,
            new Color32(223, 232, 246, 255),
            new Vector2(0f, -106f),
            new Vector2(780f, 38f),
            TextAlignmentOptions.Center);

        RectTransform menuRoot = CreateTransparentRect(
            "MenuRoot",
            canvasRect,
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, -20f),
            new Vector2(440f, 340f));
        startButton = CreateMenuButton(menuRoot, startLabel, new Vector2(0f, 0f), StartGame, true);
        CreateMenuButton(menuRoot, helpLabel, new Vector2(0f, -82f), OpenHelp, false);
        CreateMenuButton(menuRoot, quitLabel, new Vector2(0f, -154f), QuitGame, false);

        RectTransform footerRoot = CreatePanel(
            "FooterRoot",
            canvasRect,
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 42f),
            new Vector2(780f, 50f),
            new Color32(8, 14, 28, 145));
        CreateText(
            "FooterText",
            footerRoot,
            "[A][D] \uC774\uB3D9    [Space] \uC810\uD504    [C] \uC120\uD0DD",
            GetFontForText("[A][D] \uC774\uB3D9    [Space] \uC810\uD504    [C] \uC120\uD0DD"),
            24,
            FontStyles.Bold,
            new Color32(244, 247, 255, 255),
            new Vector2(0f, -8f),
            new Vector2(740f, 30f),
            TextAlignmentOptions.Center);

        helpOverlay = CreateHelpOverlay(canvasRect);
        helpOverlay.SetActive(false);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
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
        return canvas;
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
        TextAlignmentOptions alignment)
    {
        GameObject textObject = new GameObject(objectName, typeof(RectTransform));
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
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
        shadow.effectDistance = new Vector2(3f, -3f);

        return text;
    }

    private Button CreateMenuButton(RectTransform parent, string label, Vector2 anchoredPosition, UnityEngine.Events.UnityAction onClick, bool primary)
    {
        GameObject buttonObject = new GameObject(label, typeof(RectTransform));
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(360f, primary ? 62f : 56f);

        Image image = buttonObject.AddComponent<Image>();
        image.color = primary ? new Color32(255, 255, 255, 18) : new Color32(255, 255, 255, 0);

        Button button = buttonObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = primary ? new Color32(255, 255, 255, 18) : new Color32(255, 255, 255, 0);
        colors.highlightedColor = primary ? new Color32(255, 255, 255, 42) : new Color32(255, 255, 255, 14);
        colors.selectedColor = colors.highlightedColor;
        colors.pressedColor = primary ? new Color32(255, 255, 255, 70) : new Color32(255, 255, 255, 24);
        colors.disabledColor = new Color32(120, 120, 120, 90);
        colors.fadeDuration = 0.08f;
        button.colors = colors;
        button.targetGraphic = image;
        button.onClick.AddListener(onClick);

        CreateText(
            "MenuLabelShadow",
            rect,
            label,
            GetFontForText(label),
            primary ? 42 : 38,
            FontStyles.Bold,
            new Color32(30, 60, 88, 140),
            new Vector2(0f, -2f),
            new Vector2(320f, 48f),
            TextAlignmentOptions.Center);
        CreateText(
            "MenuLabel",
            rect,
            label,
            GetFontForText(label),
            primary ? 42 : 38,
            FontStyles.Bold,
            primary ? new Color32(247, 251, 255, 255) : new Color32(255, 214, 138, 255),
            new Vector2(0f, -4f),
            new Vector2(320f, 48f),
            TextAlignmentOptions.Center);

        return button;
    }

    private GameObject CreateHelpOverlay(RectTransform parent)
    {
        RectTransform overlay = CreateTransparentRect("HelpOverlay", parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        overlay.offsetMin = Vector2.zero;
        overlay.offsetMax = Vector2.zero;

        Image overlayImage = overlay.gameObject.AddComponent<Image>();
        overlayImage.color = new Color32(4, 8, 18, 196);

        RectTransform popup = CreatePanel(
            "HelpPanel",
            overlay,
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            Vector2.zero,
            new Vector2(820f, 500f),
            new Color32(11, 21, 37, 238));
        Outline outline = popup.gameObject.AddComponent<Outline>();
        outline.effectColor = new Color32(230, 202, 134, 85);
        outline.effectDistance = new Vector2(3f, -3f);

        CreateText(
            "HelpTitle",
            popup,
            helpTitle,
            GetFontForText(helpTitle),
            44,
            FontStyles.Bold,
            new Color32(248, 251, 255, 255),
            new Vector2(0f, -34f),
            new Vector2(620f, 52f),
            TextAlignmentOptions.Center);
        CreateText(
            "HelpBody",
            popup,
            "A / D : \uC774\uB3D9\nSpace : \uC810\uD504\n\n\uBAA9\uD45C\n\uAC80 3\uAC1C\uB97C \uBAA8\uC73C\uACE0, \uC801\uACFC \uD568\uC815\uC744 \uD53C\uD574\n\uBB38\uC5D0 \uB3C4\uCC29\uD558\uBA74 \uC2A4\uD14C\uC774\uC9C0\uB97C \uD074\uB9AC\uC5B4\uD569\uB2C8\uB2E4.",
            GetFontForText("A / D : \uC774\uB3D9\nSpace : \uC810\uD504\n\n\uBAA9\uD45C\n\uAC80 3\uAC1C\uB97C \uBAA8\uC73C\uACE0, \uC801\uACFC \uD568\uC815\uC744 \uD53C\uD574\n\uBB38\uC5D0 \uB3C4\uCC29\uD558\uBA74 \uC2A4\uD14C\uC774\uC9C0\uB97C \uD074\uB9AC\uC5B4\uD569\uB2C8\uB2E4."),
            27,
            FontStyles.Normal,
            new Color32(226, 233, 247, 255),
            new Vector2(0f, -112f),
            new Vector2(660f, 220f),
            TextAlignmentOptions.Center);
        CreateText(
            "HelpHint",
            popup,
            "\uC544\uC774\uD15C\uC744 \uC798 \uD65C\uC6A9\uD558\uBA74 \uB354 \uC548\uC804\uD558\uAC8C \uBE60\uC838\uB098\uAC08 \uC218 \uC788\uC2B5\uB2C8\uB2E4.",
            GetFontForText("\uC544\uC774\uD15C\uC744 \uC798 \uD65C\uC6A9\uD558\uBA74 \uB354 \uC548\uC804\uD558\uAC8C \uBE60\uC838\uB098\uAC08 \uC218 \uC788\uC2B5\uB2C8\uB2E4."),
            22,
            FontStyles.Bold,
            new Color32(255, 214, 138, 255),
            new Vector2(0f, -330f),
            new Vector2(700f, 52f),
            TextAlignmentOptions.Center);

        Button closeButton = CreateMenuButton(popup, "\uB2EB\uAE30", new Vector2(0f, -214f), CloseHelp, false);
        closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(220f, 52f);

        return overlay.gameObject;
    }

    private void CreateFullScreenImage(string objectName, RectTransform parent, Color color)
    {
        RectTransform rect = CreateTransparentRect(objectName, parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = rect.gameObject.AddComponent<Image>();
        image.color = color;
    }

    private void CreateBottomGradient(RectTransform parent)
    {
        RectTransform bottomShade = CreateTransparentRect(
            "BottomShade",
            parent,
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 0f),
            new Vector2(1920f, 280f));
        Image image = bottomShade.gameObject.AddComponent<Image>();
        image.color = new Color32(8, 12, 22, 135);
    }

    private void CreateFullScreenSprite(string objectName, RectTransform parent, Sprite sprite, Color color)
    {
        RectTransform frame = CreateTransparentRect(objectName, parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        frame.offsetMin = Vector2.zero;
        frame.offsetMax = Vector2.zero;

        AspectRatioFitter fitter = frame.gameObject.AddComponent<AspectRatioFitter>();
        fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        fitter.aspectRatio = sprite.rect.width / sprite.rect.height;

        Image image = frame.gameObject.AddComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        image.preserveAspect = true;
    }

    private Sprite LoadSpriteResource(string resourcePath)
    {
        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null)
        {
            return null;
        }

        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
    }

    private TMP_FontAsset CreateDynamicFontAsset(string resourcePath)
    {
        Font sourceFont = Resources.Load<Font>(resourcePath);
        if (sourceFont == null)
        {
            return null;
        }

        TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(sourceFont);
        fontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
        return fontAsset;
    }

    private TMP_FontAsset GetFontForText(string content)
    {
        if (ContainsHangul(content) && koreanFontAsset != null)
        {
            return koreanFontAsset;
        }

        return latinFontAsset;
    }

    private bool ContainsHangul(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return false;
        }

        for (int i = 0; i < content.Length; i++)
        {
            char current = content[i];
            if ((current >= 0x1100 && current <= 0x11FF) ||
                (current >= 0x3130 && current <= 0x318F) ||
                (current >= 0xAC00 && current <= 0xD7AF))
            {
                return true;
            }
        }

        return false;
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
