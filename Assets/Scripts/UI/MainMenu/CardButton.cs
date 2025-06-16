using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardSO cardData;
    public string CardID => cardData != null ? cardData.cardID : string.Empty;

    public Image cardImage;
    public Image cardBG;
    private Color originalColor;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardCostText;
    public TextMeshProUGUI cardATKText;

    private void Awake()
    {
        originalColor = cardBG.color;
    }

    public void Setup(CardSO card)
    {
        cardData = card;
        cardImage.sprite = card.cardImage;
        cardNameText.text = card.cardName;
        cardCostText.text = $"Cost: {card.cardCost.ToString()}";

        if (card.cardType == CardSO.CardType.Spell)
        {
            cardATKText.gameObject.SetActive(false);
        }
        else
        {
            cardATKText.text = $"ATK: {card.cardAttack.ToString()}";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardBG.color = Color.green; // Highlight color on hover
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardBG.color = originalColor; // Reset to original color when not hovering
    }
}
