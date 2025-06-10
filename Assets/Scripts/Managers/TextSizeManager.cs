using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeManager : MonoBehaviour
{
    public enum TextSize
    {
        Small,
        Medium,
        Large
    }

    public TextSize currentTextSize = TextSize.Medium;
    public float smallTextSize = 14f;
    public float mediumTextSize = 18f;
    public float largeTextSize = 22f;

    public float GetFontSize()
    {
        return currentTextSize switch
        {
            TextSize.Small => smallTextSize,
            TextSize.Medium => mediumTextSize,
            TextSize.Large => largeTextSize,
            _ => mediumTextSize, // Default to medium if not set
        };
    }

    public void SetTextSize(int dropdownValue)
    {
        currentTextSize = (TextSize)dropdownValue;
        UpdateAllTextSizes();
    }

    public void UpdateAllTextSizes()
    {
        foreach (DynamicTextSize dts in FindObjectsByType<DynamicTextSize>(FindObjectsSortMode.None))
        {
            dts.ApplyTextSize();
        }
    }
}
