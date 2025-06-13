using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class LeaderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string leaderName;
    public Image backgroundImage;
    private Color originalColor;
    public TextMeshProUGUI leaderNameText;

    private void Awake()
    {
        originalColor = backgroundImage.color;
    }

    public void Setup(string name)
    {
        leaderName = name;
        leaderNameText.text = name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundImage.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundImage.color = originalColor;
    }
}
