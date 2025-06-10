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
    public Button backButton;

    void Start()
    {
        // Volume control
        SyncSliderWithMixer(masterVolumeSlider, GameManager.Instance.AudioManager.masterVolumeParameter);
        SyncSliderWithMixer(musicVolumeSlider, GameManager.Instance.AudioManager.musicVolumeParameter);
        SyncSliderWithMixer(sfxVolumeSlider, GameManager.Instance.AudioManager.sfxVolumeParameter);
        SyncMuteWithMixer(muteToggle, GameManager.Instance.AudioManager.audioMixer);

        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);

        // Text control
        textSizeDropdown.value = (int)GameManager.Instance.TextSizeManager.currentTextSize;
        textSizeDropdown.onValueChanged.AddListener(GameManager.Instance.TextSizeManager.SetTextSize);

        //Graphics control
        graphicsDropdown.ClearOptions();
        graphicsDropdown.AddOptions(GameManager.Instance.GraphicsSettingsManager.GetQualityOptions().ToList());
        graphicsDropdown.value = GameManager.Instance.GraphicsSettingsManager.GetCurrentQualityLevel();
        graphicsDropdown.onValueChanged.AddListener(GameManager.Instance.GraphicsSettingsManager.SetQualityLevel);

        //Misc
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void SyncMuteWithMixer(Toggle muteToggle, AudioMixer audiomixer)
    {
        float currentVolume;
        audiomixer.GetFloat(GameManager.Instance.AudioManager.masterVolumeParameter, out currentVolume);
        GameManager.Instance.AudioManager.isMuted = currentVolume <= -80f;
        muteToggle.isOn = GameManager.Instance.AudioManager.isMuted;
    }

    private void SyncSliderWithMixer(Slider slider, string parameterName)
    {
        float value;
        if (GameManager.Instance.AudioManager.audioMixer.GetFloat(parameterName, out value))
        {
            slider.value = Mathf.Pow(10f, value / 20f); // Convert dB to linear scale
        }
    }

    public void OnMasterVolumeChanged(float value)
    {
        GameManager.Instance.AudioManager.SetMasterVolume(value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        GameManager.Instance.AudioManager.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        GameManager.Instance.AudioManager.SetSFXVolume(value);
    }

    public void OnMuteToggleChanged(bool isMuted)
    {
        GameManager.Instance.AudioManager.MuteAll(isMuted);
    }

    private void OnBackClicked()
    {
        GameManager.Instance.UIManager.HidePanel("SettingsMenu");
        GameManager.Instance.UIManager.ShowPanel("MainMenu");
    }
}
