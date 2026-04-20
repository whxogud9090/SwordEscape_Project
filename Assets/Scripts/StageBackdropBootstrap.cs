using UnityEngine;
using UnityEngine.SceneManagement;

public static class StageBackdropBootstrap
{
    private const string BackdropName = "RuntimeStageBackdrop";
    private const int TextureWidth = 96;
    private const int TextureHeight = 128;
    private const float PixelsPerUnit = 32f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        ApplyBackdrop(SceneManager.GetActiveScene());
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyBackdrop(scene);
    }

    private static void ApplyBackdrop(Scene scene)
    {
        if (!TryGetPalette(scene.name, out StageBackdropPalette palette))
        {
            return;
        }

        Camera cameraRef = Camera.main;
        if (cameraRef == null)
        {
            return;
        }

        Transform existing = cameraRef.transform.Find(BackdropName);
        GameObject backdropObject;
        SpriteRenderer spriteRenderer;

        if (existing != null)
        {
            backdropObject = existing.gameObject;
            spriteRenderer = backdropObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = backdropObject.AddComponent<SpriteRenderer>();
            }
        }
        else
        {
            backdropObject = new GameObject(BackdropName);
            backdropObject.transform.SetParent(cameraRef.transform, false);
            spriteRenderer = backdropObject.AddComponent<SpriteRenderer>();
        }

        Texture2D texture = BuildTexture(palette);
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            PixelsPerUnit);

        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = -500;
        spriteRenderer.color = Color.white;

        backdropObject.transform.localPosition = new Vector3(0f, 0f, 30f);
        backdropObject.transform.localRotation = Quaternion.identity;
        backdropObject.transform.localScale = CalculateScale(cameraRef, spriteRenderer.sprite);

        cameraRef.clearFlags = CameraClearFlags.SolidColor;
        cameraRef.backgroundColor = palette.BottomColor;
    }

    private static Texture2D BuildTexture(StageBackdropPalette palette)
    {
        Texture2D texture = new Texture2D(TextureWidth, TextureHeight, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < TextureHeight; y++)
        {
            float t = y / (float)(TextureHeight - 1);
            Color gradient = Color.Lerp(palette.BottomColor, palette.TopColor, t);

            for (int x = 0; x < TextureWidth; x++)
            {
                float brick = Mathf.PerlinNoise((x + palette.NoiseOffset) * 0.08f, (y + palette.NoiseOffset) * 0.06f);
                float band = Mathf.PerlinNoise((x + 31f + palette.NoiseOffset) * 0.03f, (y + 17f + palette.NoiseOffset) * 0.11f);
                float vignette = Mathf.Abs((x / (float)(TextureWidth - 1)) - 0.5f) * 0.08f;

                Color finalColor = gradient;
                finalColor *= 0.96f + (brick * 0.08f);
                finalColor *= 0.98f + (band * 0.04f);
                finalColor *= 1f - vignette;
                finalColor.a = 1f;

                texture.SetPixel(x, y, finalColor);
            }
        }

        texture.Apply();
        return texture;
    }

    private static Vector3 CalculateScale(Camera cameraRef, Sprite sprite)
    {
        float worldHeight = cameraRef.orthographicSize * 2f;
        float worldWidth = worldHeight * cameraRef.aspect;
        Vector2 spriteSize = sprite.bounds.size;

        return new Vector3(
            (worldWidth / spriteSize.x) * 1.05f,
            (worldHeight / spriteSize.y) * 1.05f,
            1f);
    }

    private static bool TryGetPalette(string sceneName, out StageBackdropPalette palette)
    {
        switch (sceneName)
        {
            case "Stage1":
                palette = new StageBackdropPalette(
                    new Color(0.66f, 0.68f, 0.62f),
                    new Color(0.86f, 0.84f, 0.76f),
                    11f);
                return true;
            case "Stage2":
                palette = new StageBackdropPalette(
                    new Color(0.53f, 0.54f, 0.48f),
                    new Color(0.72f, 0.69f, 0.60f),
                    37f);
                return true;
            case "Stage3":
                palette = new StageBackdropPalette(
                    new Color(0.37f, 0.36f, 0.39f),
                    new Color(0.50f, 0.48f, 0.53f),
                    61f);
                return true;
            case "Stage4":
                palette = new StageBackdropPalette(
                    new Color(0.24f, 0.24f, 0.28f),
                    new Color(0.35f, 0.35f, 0.41f),
                    89f);
                return true;
            case "Stage5":
                palette = new StageBackdropPalette(
                    new Color(0.31f, 0.22f, 0.24f),
                    new Color(0.54f, 0.33f, 0.30f),
                    123f);
                return true;
            default:
                palette = default;
                return false;
        }
    }

    private readonly struct StageBackdropPalette
    {
        public StageBackdropPalette(Color bottomColor, Color topColor, float noiseOffset)
        {
            BottomColor = bottomColor;
            TopColor = topColor;
            NoiseOffset = noiseOffset;
        }

        public Color BottomColor { get; }
        public Color TopColor { get; }
        public float NoiseOffset { get; }
    }
}
