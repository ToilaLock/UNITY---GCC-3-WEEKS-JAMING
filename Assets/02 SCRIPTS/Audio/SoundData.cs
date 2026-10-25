using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    Master,
    Music,
    SFX,
    Voice
}

[CreateAssetMenu(fileName = "NewSoundData", menuName = "Audio/Sound Data")]
public class SoundData : ScriptableObject
{
    [Header("Audio Info")]
    public string soundID;
    public AudioClip clip;
    public SoundType soundType = SoundType.SFX;

    [Header("Parameters")]
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;

    [Header("Pitch Randomization")]
    public bool useRandomPitch = false;
    [Range(0f, 0.5f)] public float pitchVariation = 0.05f;
}