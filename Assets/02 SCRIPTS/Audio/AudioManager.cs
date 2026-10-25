using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup voiceGroup;

    [Header("Music Player")]
    [SerializeField] private AudioSource musicSource;

    [Header("SFX Pooling")]
    [SerializeField] private int initialPoolSize = 10;
    private List<AudioSource> sfxSourcePool = new List<AudioSource>();

    [Header("Predefined Sounds")]
    [SerializeField] private List<SoundData> soundDatabase = new List<SoundData>();
    private Dictionary<string, SoundData> soundDict = new Dictionary<string, SoundData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSoundDict();
        InitializePool();
    }

    private void InitializeSoundDict()
    {
        foreach (var data in soundDatabase)
        {
            if (data != null && !string.IsNullOrEmpty(data.soundID))
            {
                if (!soundDict.ContainsKey(data.soundID))
                    soundDict.Add(data.soundID, data);
            }
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreatePooledSource();
        }
    }

    private AudioSource CreatePooledSource()
    {
        GameObject obj = new GameObject("PooledAudioSource");
        obj.transform.SetParent(transform);
        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        sfxSourcePool.Add(source);
        return source;
    }

    private AudioSource GetAvailableSource()
    {
        for (int i = 0; i < sfxSourcePool.Count; i++)
        {
            if (!sfxSourcePool[i].isPlaying)
                return sfxSourcePool[i];
        }

        return CreatePooledSource();
    }

    private AudioMixerGroup GetGroup(SoundType type)
    {
        return type switch
        {
            SoundType.Master => masterGroup,
            SoundType.Music => musicGroup,
            SoundType.SFX => sfxGroup,
            SoundType.Voice => voiceGroup,
            _ => sfxGroup
        };
    }

    public void PlaySound(SoundData data)
    {
        if (data == null || data.clip == null) return;

        if (data.soundType == SoundType.Music)
        {
            PlayMusic(data);
            return;
        }

        AudioSource source = GetAvailableSource();
        source.clip = data.clip;
        source.outputAudioMixerGroup = GetGroup(data.soundType);
        source.volume = data.volume;
        source.loop = data.loop;

        float finalPitch = data.pitch;
        if (data.useRandomPitch)
        {
            finalPitch += Random.Range(-data.pitchVariation, data.pitchVariation);
        }
        source.pitch = finalPitch;

        source.Play();
    }

    public void PlaySound(string soundID)
    {
        if (soundDict.TryGetValue(soundID, out SoundData data))
        {
            PlaySound(data);
        }
        else
        {
            // Debug.LogWarning($"[AudioManager] Sound ID not found: {soundID}");
        }
    }

    public void PlayOneShot(AudioClip clip, SoundType soundType = SoundType.SFX, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        source.clip = clip;
        source.outputAudioMixerGroup = GetGroup(soundType);
        source.volume = volume;
        source.loop = false;
        source.pitch = 1f;
        source.Play();
    }

    public void PlayMusic(SoundData musicData)
    {
        if (musicData == null || musicData.clip == null) return;

        if (musicSource == null)
        {
            GameObject obj = new GameObject("MusicSource");
            obj.transform.SetParent(transform);
            musicSource = obj.AddComponent<AudioSource>();
        }

        musicSource.clip = musicData.clip;
        musicSource.outputAudioMixerGroup = GetGroup(SoundType.Music);
        musicSource.volume = musicData.volume;
        musicSource.loop = true;
        musicSource.pitch = 1f;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }

    public void SetVolume(string exposedParam, float normalizedVolume)
    {
        float db = normalizedVolume > 0.0001f ? Mathf.Log10(normalizedVolume) * 20f : -80f;
        mainMixer.SetFloat(exposedParam, db);
    }
}