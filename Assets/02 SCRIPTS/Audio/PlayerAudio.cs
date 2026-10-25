using System;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Sound IDs")]
    [SerializeField] private string footstepSoundID = "player_footstep";
    [SerializeField] private string attack1SoundID = "player_attack_1";
    [SerializeField] private string attack2SoundID = "player_attack_2";
    [SerializeField] private string attack3SoundID = "player_attack_3";
    [SerializeField] private string hurtSoundID = "player_hurt";
    [SerializeField] private string rollSoundID = "player_roll";
    [SerializeField] private string voiceJumpSoundID = "player_voice_jump";
    [SerializeField] private string jumpSoundID = "player_jump";
    [SerializeField] private string voiceDeadSoundID = "player_voice_dead";

    // STATE 
    public void PlayAttack1Sound() => AudioManager.Instance?.PlaySound(attack1SoundID);
    public void PlayAttack2Sound() => AudioManager.Instance?.PlaySound(attack2SoundID);
    public void PlayAttack3Sound() => AudioManager.Instance?.PlaySound(attack3SoundID);

    public void PlayJumpSound()
    {
        AudioManager.Instance?.PlaySound(voiceJumpSoundID);
        AudioManager.Instance?.PlaySound(jumpSoundID);
    }

    public void PlayHurtSound() => AudioManager.Instance?.PlaySound(hurtSoundID);
    public void PlayRollSound() => AudioManager.Instance?.PlaySound(rollSoundID);

    public void PlayDeadSound()
    {
        AudioManager.Instance?.PlaySound(voiceDeadSoundID);
        AudioManager.Instance?.PlaySound(attack3SoundID);
    }


    //ANIM EVENT
    public void AnimEven_FootSound() => AudioManager.Instance?.PlaySound(footstepSoundID);
}