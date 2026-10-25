using System.ComponentModel;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [Header("Sound IDs")]
    [SerializeField] private string backGroundMusic = "music_background";

    public void PlayBackgroundMusic() => AudioManager.Instance.PlaySound(backGroundMusic);
}