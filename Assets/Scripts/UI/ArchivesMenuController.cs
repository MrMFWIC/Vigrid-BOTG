using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArchivesMenuController : MonoBehaviour
{
    public TMP_InputField searchInputField;
    public TMP_Dropdown cardTypeFilterDropdown;
    public TMP_Dropdown affiliationFilterDropdown;
    public TMP_Dropdown sortDropdown;
    public Transform cardGridParent;
    public GameObject cardButtonPrefab;

    public List<CardSO> allCards;
    private List<GameObject> currentButtons = new List<GameObject>();

    private void Start()
    {
        LoadAllCards();
        UpdateCardDisplay();
        searchInputField.onValueChanged.AddListener(_ => UpdateCardDisplay());
        cardTypeFilterDropdown.onValueChanged.AddListener(_ => UpdateCardDisplay());
        affiliationFilterDropdown.onValueChanged.AddListener(_ => UpdateCardDisplay());
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
            .Where(card => PassesCardTypeFilter(card))
            .Where(card => PassesAffiliationFilter(card))
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

    bool PassesCardTypeFilter(CardSO card)
    {
        switch (cardTypeFilterDropdown.value)
        {
            case 0: // All
                return true;
            case 1: // Creature
                return card.cardType == CardSO.CardType.Unit;
            case 2: // Spell
                return card.cardType == CardSO.CardType.Spell;
            default:
                return true;
        }
    }

    bool PassesAffiliationFilter(CardSO card)
    {
        switch (affiliationFilterDropdown.value)
        {
            case 0: // All
                return true;
            case 1: // Affiliation
                return card.affiliation == CardSO.Affiliation.Human; // Example for Human, can expand
            case 2:
                return card.affiliation == CardSO.Affiliation.Dark; // Example for Dark, can expand
            case 3:
                return card.affiliation == CardSO.Affiliation.Mystic; // Example for Mystic, can expand
            case 4:
                return card.affiliation == CardSO.Affiliation.Beast; // Example for Beast, can expand
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
