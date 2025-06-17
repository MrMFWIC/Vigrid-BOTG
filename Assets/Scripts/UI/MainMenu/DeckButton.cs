using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class DeckButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string deckName;
    public Image backgroundImage;
    public Image deckImage;
    private Color originalColor;
    public TextMeshProUGUI deckNameText;

    private bool isSelected = false;

    private void Awake()
    {
        originalColor = backgroundImage.color;
    }

    public void Setup(string name)
    {
        deckName = name;
        deckNameText.text = name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected) return;
        backgroundImage.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) return;
        backgroundImage.color = originalColor;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }
}
