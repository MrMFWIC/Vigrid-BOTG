using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardSO cardInstance;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardEffectText;
    [SerializeField] private TextMeshProUGUI cardCostText;
    [SerializeField] private TextMeshProUGUI cardAttackText;

    [Header("Images")]
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardBackground;

    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        if (cardInstance != null)
        {
            cardImage.sprite = cardInstance.cardImage;
            cardNameText.text = cardInstance.cardName;
            cardEffectText.text = cardInstance.cardEffect;
            cardCostText.text = $"Cost: {cardInstance.cardCost}";
            cardBackground.color = cardInstance.GetAffiliationColor();

            switch (cardInstance.cardType)
            {
                case CardSO.CardType.Unit:
                    cardAttackText.text = $"ATK: {cardInstance.cardAttack}";
                    break;
                case CardSO.CardType.Spell:
                    cardAttackText.text = $"SPELL";
                    break;
                default:
                    Debug.LogError("Card type not recognized.");
                    break;
            }
        }
        else
        {
            Debug.LogError("CardSO is not assigned.");
        }
    }
}