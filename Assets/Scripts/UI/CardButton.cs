using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardButton : MonoBehaviour
{
    private CardSO cardData;

    public Image cardImage;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardCostText;
    public TextMeshProUGUI cardATKText;

    public void Setup(CardSO card)
    {
        cardData = card;
        cardImage.sprite = card.cardImage;
        cardNameText.text = card.cardName;
        cardCostText.text = card.cardCost.ToString();
        cardATKText.text = card.cardAttack.ToString();
    }
}
