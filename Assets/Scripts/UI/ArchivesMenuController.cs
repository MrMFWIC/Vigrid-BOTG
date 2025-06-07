using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArchivesMenuController : MonoBehaviour
{
    public InputField searchInputField;
    public Dropdown filterDropdown;
    public Dropdown sortDropdown;
    public Transform cardGridParent;
    public GameObject cardButtonPrefab;

    public List<CardSO> allCards;
    private List<GameObject> currentButtons = new List<GameObject>();

    private void Start()
    {
        LoadAllCards();
        UpdateCardDisplay();
        searchInputField.onValueChanged.AddListener(_ => UpdateCardDisplay());
        filterDropdown.onValueChanged.AddListener(_ => UpdateCardDisplay());
        sortDropdown.onValueChanged.AddListener(_ => UpdateCardDisplay());
    }

    void LoadAllCards()
    {
        allCards = Resources.LoadAll<CardSO>("Cards").ToList();
    }

    void UpdateCardDisplay()
    {
        foreach (var button in currentButtons)
        {
            Destroy(button);
        }
        currentButtons.Clear();

        var filtered = allCards
            .Where(card => card.cardName.ToLower().Contains(searchInputField.text.ToLower()))
            .Where(card => PassesFilter(card))
            .OrderBy(card => GetSortValue(card))
            .ToList();

        foreach (var card in filtered)
        {
            GameObject button = CreateCardButton(card);
            currentButtons.Add(button);
        }
    }

    GameObject CreateCardButton(CardSO card)
    {
        var buttonGO = Instantiate(cardButtonPrefab, cardGridParent);
        
        var cardButton = buttonGO.GetComponent<CardButton>();
        if (cardButton != null)
        {
            cardButton.Setup(card);
        }

        buttonGO.GetComponent<Button>().onClick.AddListener(() => ShowCardDetails(card));

        return buttonGO;
    }

    bool PassesFilter(CardSO card)
    {
        switch (filterDropdown.value)
        {
            case 0: // All
                return true;
            case 1: // Unit
                return card.cardType == CardSO.CardType.Unit;
            case 2: // Spell
                return card.cardType == CardSO.CardType.Spell;
            case 3: // Affiliation
                return card.affiliation == CardSO.Affiliation.Human; // Example for Human, can expand
            default:
                return true;
        }
    }

    object GetSortValue(CardSO card)
    {
        switch (sortDropdown.value)
        {
            case 0: // Name
                return card.cardName;
            case 1: // Cost
                return card.cardCost;
            case 2: // Attack
                return card.cardAttack;
            default:
                return card.cardName;
        }
    }

    void ShowCardDetails(CardSO card)
    {
        // Implement the logic to show card details, e.g., open a new panel with card info
        Debug.Log($"Showing details for {card.cardName}");
    }
}
