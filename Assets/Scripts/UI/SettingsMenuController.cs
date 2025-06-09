using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SettingsMenuController : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Toggle muteToggle;
    public TMP_Dropdown textSizeDropdown;
    public TMP_Dropdown graphicsDropdown;

    void Start()
    {
        // Volume control
        SyncSliderWithMixer(masterVolumeSlider, AudioManager.Instance.masterVolumeParameter);
        SyncSliderWithMixer(musicVolumeSlider, AudioManager.Instance.musicVolumeParameter);
        SyncSliderWithMixer(sfxVolumeSlider, AudioManager.Instance.sfxVolumeParameter);
        SyncMuteWithMixer(muteToggle, AudioManager.Instance.audioMixer);
        
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);

        // Text control
        textSizeDropdown.value = (int)TextSizeManager.Instance.currentTextSize;
        textSizeDropdown.onValueChanged.AddListener(TextSizeManager.Instance.SetTextSize);

        //Graphics control
        graphicsDropdown.ClearOptions();
        graphicsDropdown.AddOptions(GraphicsSettingsManager.Instance.GetQualityOptions().ToList());
        graphicsDropdown.value = GraphicsSettingsManager.Instance.GetCurrentQualityLevel();
        graphicsDropdown.onValueChanged.AddListener(GraphicsSettingsManager.Instance.SetQualityLevel);
    }

    private void SyncMuteWithMixer(Toggle muteToggle, AudioMixer audiomixer)
    {
        float currentVolume;
        audiomixer.GetFloat(AudioManager.Instance.masterVolumeParameter, out currentVolume);
        AudioManager.Instance.isMuted = currentVolume <= -80f;
        muteToggle.isOn = AudioManager.Instance.isMuted;
    }

    private void SyncSliderWithMixer(Slider slider, string parameterName)
    {
        float value;
        if (AudioManager.Instance.audioMixer.GetFloat(parameterName, out value))
        {
            slider.value = Mathf.Pow(10f, value / 20f); // Convert dB to linear scale
        }
    }

    public void OnMasterVolumeChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void OnMuteToggleChanged(bool isMuted)
    {
        AudioManager.Instance.MuteAll(isMuted);
    }
}
