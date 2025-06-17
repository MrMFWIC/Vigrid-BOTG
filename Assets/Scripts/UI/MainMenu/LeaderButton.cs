using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class LeaderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public LeaderSO leaderData;

    public string leaderName;
    public Image backgroundImage;
    public Image leaderImage;
    private Color originalColor;
    public TextMeshProUGUI leaderNameText;

    private bool isSelected = false;

    private void Awake()
    {
        originalColor = backgroundImage.color;
    }

    public void Setup(LeaderSO leader)
    {
        leaderData = leader;
        leaderName = leader.leaderName;
        leaderNameText.text = leaderName;
        leaderImage.sprite = leader.leaderImage;
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
