using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class AudioBootstrap : MonoBehaviour
{
    public static AudioBootstrap Instance { get; private set; }

    private AudioSource sfxSource;
    private AudioSource musicSource;

    private AudioClip jumpClip;
    private AudioClip itemClip;
    private AudioClip swordClip;
    private AudioClip doorClip;
    private AudioClip deathClip;
    private AudioClip clearClip;
    private AudioClip bossMusicClip;
    private bool clearPlayedThisScene;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        if (Instance != null)
        {
            Instance.HandleSceneChanged(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            return;
        }

        GameObject audioObject = new GameObject("AudioBootstrap");
        audioObject.AddComponent<AudioBootstrap>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.volume = 0.65f;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = 0.24f;

        BuildClips();

        SceneManager.sceneLoaded -= HandleSceneChanged;
        SceneManager.sceneLoaded += HandleSceneChanged;
        HandleSceneChanged(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= HandleSceneChanged;
            Instance = null;
        }
    }

    public void PlayJump()
    {
        PlayOneShot(jumpClip, 0.65f);
    }

    public void PlayItemPickup()
    {
        PlayOneShot(itemClip, 0.62f);
    }

    public void PlaySwordPickup(bool isFinalSword)
    {
        PlayOneShot(isFinalSword ? clearClip : swordClip, isFinalSword ? 0.9f : 0.7f);
    }

    public void PlayDoorOpen()
    {
        PlayOneShot(doorClip, 0.75f);
    }

    public void PlayDeath()
    {
        PlayOneShot(deathClip, 0.8f);
    }

    public void PlayClear()
    {
        if (clearPlayedThisScene)
        {
            return;
        }

        clearPlayedThisScene = true;
        PlayOneShot(clearClip, 0.95f);
        StopBossMusicSmooth(0.35f);
    }

    private void HandleSceneChanged(Scene scene, LoadSceneMode mode)
    {
        clearPlayedThisScene = false;

        if (scene.name == "Stage5")
        {
            StartBossMusic();
        }
        else
        {
            StopBossMusicImmediate();
        }
    }

    private void StartBossMusic()
    {
        if (bossMusicClip == null)
        {
            return;
        }

        if (musicSource.clip == bossMusicClip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.Stop();
        musicSource.clip = bossMusicClip;
        musicSource.volume = 0.24f;
        musicSource.Play();
    }

    private void StopBossMusicImmediate()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }

    private void StopBossMusicSmooth(float duration)
    {
        if (!musicSource.isPlaying)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FadeOutMusic(duration));
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = null;
        musicSource.volume = startVolume;
    }

    private void PlayOneShot(AudioClip clip, float volumeScale)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip, volumeScale);
    }

    private void BuildClips()
    {
        jumpClip = CreateSweepClip("JumpSfx", 0.12f, 760f, 980f, 0.18f);
        itemClip = CreateSweepClip("ItemPickupSfx", 0.12f, 540f, 820f, 0.18f);
        swordClip = CreateChimeClip("SwordPickupSfx", 0.18f, 920f, 1320f);
        doorClip = CreateSweepClip("DoorSfx", 0.2f, 320f, 180f, 0.22f);
        deathClip = CreateSweepClip("DeathSfx", 0.28f, 300f, 120f, 0.3f);
        clearClip = CreateVictoryClip("ClearSfx");
        bossMusicClip = CreateBossLoop("BossLoop");
    }

    private AudioClip CreateSweepClip(string clipName, float duration, float startFrequency, float endFrequency, float amplitude)
    {
        int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleCount;
            float frequency = Mathf.Lerp(startFrequency, endFrequency, t);
            float envelope = Mathf.Sin(t * Mathf.PI);
            float wave = Mathf.Sin(2f * Mathf.PI * frequency * i / sampleRate);
            samples[i] = wave * envelope * amplitude;
        }

        return CreateClip(clipName, samples, sampleRate);
    }

    private AudioClip CreateChimeClip(string clipName, float duration, float frequencyA, float frequencyB)
    {
        int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleCount;
            float envelope = Mathf.Pow(1f - t, 2.2f);
            float waveA = Mathf.Sin(2f * Mathf.PI * frequencyA * i / sampleRate);
            float waveB = Mathf.Sin(2f * Mathf.PI * frequencyB * i / sampleRate);
            samples[i] = (waveA * 0.65f + waveB * 0.35f) * envelope * 0.24f;
        }

        return CreateClip(clipName, samples, sampleRate);
    }

    private AudioClip CreateVictoryClip(string clipName)
    {
        int sampleRate = 44100;
        float[] noteDurations = { 0.12f, 0.12f, 0.22f };
        float[] noteFrequencies = { 740f, 932f, 1244f };
        int sampleCount = 0;

        for (int i = 0; i < noteDurations.Length; i++)
        {
            sampleCount += Mathf.CeilToInt(noteDurations[i] * sampleRate);
        }

        float[] samples = new float[sampleCount];
        int cursor = 0;

        for (int n = 0; n < noteDurations.Length; n++)
        {
            int noteSamples = Mathf.CeilToInt(noteDurations[n] * sampleRate);
            for (int i = 0; i < noteSamples; i++)
            {
                float t = i / (float)noteSamples;
                float envelope = Mathf.Sin(t * Mathf.PI) * (1f - (t * 0.15f));
                float wave = Mathf.Sin(2f * Mathf.PI * noteFrequencies[n] * i / sampleRate);
                samples[cursor + i] = wave * envelope * 0.24f;
            }

            cursor += noteSamples;
        }

        return CreateClip(clipName, samples, sampleRate);
    }

    private AudioClip CreateBossLoop(string clipName)
    {
        int sampleRate = 44100;
        float duration = 3.2f;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];
        float[] pulseFrequencies = { 110f, 110f, 146.83f, 110f, 174.61f, 146.83f, 110f, 98f };
        int pulseCount = pulseFrequencies.Length;

        for (int i = 0; i < sampleCount; i++)
        {
            float time = i / (float)sampleRate;
            int pulseIndex = Mathf.FloorToInt((time / duration) * pulseCount) % pulseCount;
            float pulseFrequency = pulseFrequencies[pulseIndex];
            float pulsePhase = (time * pulseCount / duration) % 1f;
            float pulseEnvelope = pulsePhase < 0.18f ? 1f - (pulsePhase / 0.18f) : 0f;

            float bass = Mathf.Sin(2f * Mathf.PI * pulseFrequency * time) * 0.16f * pulseEnvelope;
            float drone = Mathf.Sin(2f * Mathf.PI * 55f * time) * 0.05f;
            float hiss = (Mathf.PerlinNoise(time * 9f, 0f) - 0.5f) * 0.02f;
            samples[i] = bass + drone + hiss;
        }

        return CreateClip(clipName, samples, sampleRate);
    }

    private AudioClip CreateClip(string clipName, float[] samples, int sampleRate)
    {
        AudioClip clip = AudioClip.Create(clipName, samples.Length, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
