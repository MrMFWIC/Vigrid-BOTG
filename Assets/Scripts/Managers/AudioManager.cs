using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string masterVolumeParameter = "MasterVolume";
    public string musicVolumeParameter = "MusicVolume";
    public string sfxVolumeParameter = "SFXVolume";
    private float previousMasterVolume = 0f;
    public bool isMuted = false;

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat(masterVolumeParameter, VolumeToDecibels(volume));
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicVolumeParameter, VolumeToDecibels(volume));
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(sfxVolumeParameter, VolumeToDecibels(volume));
    }

    public void MuteAll(bool mute)
    {
        isMuted = mute;

        if (mute)
        {
            if (audioMixer.GetFloat(masterVolumeParameter, out float currentVolume))
            {
                previousMasterVolume = currentVolume;
            }

            audioMixer.SetFloat(masterVolumeParameter, -80f); //Effectively mute
        }
        else
        {
            audioMixer.SetFloat(masterVolumeParameter, previousMasterVolume);
        }
    }

    private float VolumeToDecibels(float volume)
    {
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
    }

    public bool IsMuted()
    {
        return isMuted;
    }
}
