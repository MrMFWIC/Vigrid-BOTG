using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsSettingsManager : MonoBehaviour
{
    public string[] GetQualityOptions()
    {
        return QualitySettings.names;
    }

    public int GetCurrentQualityLevel()
    {
        return PlayerPrefs.HasKey("GraphicsQuality")
            ? PlayerPrefs.GetInt("GraphicsQuality")
            : QualitySettings.GetQualityLevel();
    }

    public void SetQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level, true);
        PlayerPrefs.SetInt("GraphicsQuality", level);
        PlayerPrefs.Save();
    }
}
