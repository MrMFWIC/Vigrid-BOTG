using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ArchivesMenuController : MonoBehaviour
{
    [Header("Card Databank")]
    private CardDatabase cardDatabase;
    public TMP_InputField searchInputField;
    public TMP_Dropdown cardTypeFilterDropdown;
    public TMP_Dropdown affiliationFilterDropdown;
    public TMP_Dropdown sortDropdown;
    public Transform cardGridParent;
    public GameObject cardButtonPrefab;
    public List<CardSO> allCards;
    private List<GameObject> currentButtons = new List<GameObject>();

    [Header("Deck Creation")]
    public TMP_InputField deckNameInputField;
    public Button saveDeckButton;
    public Button clearDeckButton;
    public Button deleteDeckButton;
    public Transform deckCardGridParent;
    public TMP_Dropdown savedDecksDropdown;
    public List<CardSO> deckCards;
    private List<GameObject> currentDeckButtons = new List<GameObject>();
    private string currentLoadedDeckName = null;
    public int currentDeckSize;
    public TextMeshProUGUI currentDeckSizeText;
    public int currentDeckAverageCost;
    public TextMeshProUGUI currentDeckAverageCostText;
    public int currentDeckSpellCount;
    public TextMeshProUGUI currentDeckSpellCountText;
    public int currentDeckUnitCount;
    public TextMeshProUGUI currentDeckUnitCountText;

    [Header("Card Details")]
    public GameObject cardDetailsPanel;
    public CardSO selectedCard;
    public GameObject cardPrefabObj;
    public Button addToDeckButton;
    public Button removeFromDeckButton;
    public TextMeshProUGUI cardLoreText;

    [Header("Misc")]
    public Button backButton;
    private int maxDeckSize = 40;
    private int minDeckSize = 30;
    private int maxIdenticalCards = 3;

    private void Start()
    {
        //Card Databank Initialization
        cardDatabase = Resources.Load<CardDatabase>("CardDatabase");
        LoadAllCards();
        UpdateCardDisplay();
        searchInputField.onValueChanged.AddListener(_ => UpdateCardDisplay());
        cardTypeFilterDropdown.onValueChanged.AddListener(_ => UpdateCardDisplay());
        affiliationFilterDropdown.onValueChanged.AddListener(_ => UpdateCardDisplay());
        sortDropdown.onValueChanged.AddListener(_ => UpdateCardDisplay());

        //Deck Creation Initialization
        UpdateDeckDetails();
        PopulateSavedDecksDropdown();
        savedDecksDropdown.onValueChanged.AddListener(OnSavedDeckSelected);
        saveDeckButton.onClick.AddListener(OnSaveDeckClicked);
        clearDeckButton.onClick.AddListener(OnClearDeckClicked);
        deleteDeckButton.onClick.AddListener(OnDeleteDeckClicked);

        //Card Details Initialization
        cardDetailsPanel.SetActive(false);

        //Misc Initialization
        backButton.onClick.AddListener(OnBackClicked);

    }

    void LoadAllCards()
    {
        allCards = Resources.LoadAll<CardSO>("Cards").ToList();
    }

    void PopulateSavedDecksDropdown()
    {
        savedDecksDropdown.ClearOptions();

        // Get saved deck names
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
        List<string> deckNames = files
            .Select(f => Path.GetFileNameWithoutExtension(f))
            .ToList();

        // Add "New Deck" as first option
        List<string> options = new List<string> { "New Deck" };
        options.AddRange(deckNames);

        savedDecksDropdown.AddOptions(options);
        savedDecksDropdown.interactable = true;
        deleteDeckButton.interactable = deckNames.Count > 0;

        // Select the loaded deck or default to "New Deck"
        if (!string.IsNullOrEmpty(currentLoadedDeckName))
        {
            int index = options.IndexOf(currentLoadedDeckName);
            if (index != -1)
            {
                savedDecksDropdown.value = index;
            }
            else
            {
                savedDecksDropdown.value = 0; // "New Deck"
            }
        }
        else
        {
            savedDecksDropdown.value = 0; // "New Deck"
        }

        savedDecksDropdown.RefreshShownValue();
    }

    void OnSavedDeckSelected(int index)
    {
        string selectedDeckName = savedDecksDropdown.options[index].text;

        if (selectedDeckName == "New Deck")
        {
            OnClearDeckClicked();
            deckNameInputField.text = "";
            currentLoadedDeckName = null;
            return;
        }

        LoadDeckFromFile(selectedDeckName);
    }

    void UpdateDeckDetails()
    {
        currentDeckSize = 0;
        currentDeckSize = currentDeckButtons.Count;
        currentDeckSizeText.text = $"Deck Size: {currentDeckSize}";

        currentDeckAverageCost = 0;
        currentDeckAverageCost = currentDeckSize > 0 ? (int)deckCards.Average(card => card.cardCost) : 0;
        currentDeckAverageCostText.text = $"Cost Avg: {currentDeckAverageCost}";

        currentDeckSpellCount = 0;
        currentDeckSpellCount = deckCards.Count(card => card.cardType == CardSO.CardType.Spell);
        currentDeckSpellCountText.text = $"Spells: {currentDeckSpellCount}";

        currentDeckUnitCount = 0;
        currentDeckUnitCount = deckCards.Count(card => card.cardType == CardSO.CardType.Unit);
        currentDeckUnitCountText.text = $"Units: {currentDeckUnitCount}";
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

        buttonGO.GetComponent<Button>().onClick.AddListener(() => ShowCardDetails(card, buttonGO));

        return buttonGO;
    }

    GameObject CreateDeckCardButton(CardSO card)
    {
        var deckButtonGO = Instantiate(cardButtonPrefab, deckCardGridParent);

        var cardButton = deckButtonGO.GetComponent<CardButton>();
        if (cardButton != null)
        {
            cardButton.Setup(card);
        }

        deckButtonGO.GetComponent<Button>().onClick.AddListener(() => ShowCardDetails(card, deckButtonGO));

        return deckButtonGO;
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

    void ShowCardDetails(CardSO card, GameObject sourceButton = null)
    {
        cardDetailsPanel.SetActive(true);
        selectedCard = card;
        addToDeckButton.onClick.RemoveAllListeners();
        addToDeckButton.onClick.AddListener(() => OnAddToDeckClicked(card));

        removeFromDeckButton.onClick.RemoveAllListeners();

        bool isInDeck = deckCards.Any(c => c.cardID == card.cardID);
        removeFromDeckButton.gameObject.SetActive(isInDeck);
        Debug.Log($"Card {card.cardID} is in deck: {isInDeck}");

        if (isInDeck)
        {
            removeFromDeckButton.onClick.AddListener(() => OnRemoveFromDeckClicked(card.cardID));
        }

        cardPrefabObj.GetComponent<CardDisplay>().cardInstance = selectedCard;
        cardPrefabObj.GetComponent<CardDisplay>().UpdateCardDisplay();
        cardLoreText.text = selectedCard.cardLore;
    }

    private void OnBackClicked()
    {
        cardDetailsPanel.SetActive(false);
        GameManager.Instance.UIManager.HidePanel("ArchivesMenu");
        GameManager.Instance.UIManager.ShowPanel("MainMenu");
    }

    private void OnClearDeckClicked()
    {
        foreach (var button in currentDeckButtons)
        {
            Destroy(button);
        }
        currentDeckButtons.Clear();
        deckCards.Clear();
        UpdateDeckDetails();
    }

    private void OnSaveDeckClicked()
    {
        if (currentDeckSize < minDeckSize || currentDeckSize > maxDeckSize)
        {
            Debug.LogWarning($"Deck size must be between {minDeckSize} and {maxDeckSize}.");
            return;
        }

        string newDeckName = deckNameInputField.text.Trim();
        if (string.IsNullOrEmpty(newDeckName))
        {
            Debug.LogWarning("Deck name cannot be empty.");
            return;
        }

        string newPath = Path.Combine(Application.persistentDataPath, newDeckName + ".json");
        bool deckAlreadyExists = File.Exists(newPath);
        bool isRenamingDeck = currentLoadedDeckName != null && newDeckName != currentLoadedDeckName;

        // Prevent name collision with other decks
        if (deckAlreadyExists && !isRenamingDeck)
        {
            Debug.LogWarning($"A deck named '{newDeckName}' already exists.");
            return;
        }

        // If renaming, delete the old file
        if (isRenamingDeck)
        {
            string oldPath = Path.Combine(Application.persistentDataPath, currentLoadedDeckName + ".json");
            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
                Debug.Log($"Renamed deck from '{currentLoadedDeckName}' to '{newDeckName}'");
            }
        }

        // Save new/updated deck
        SavedDeck data = new SavedDeck
        {
            deckName = newDeckName,
            cardIDs = deckCards.Select(card => card.cardID).ToList()
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(newPath, json);
        Debug.Log($"Deck '{newDeckName}' saved with {currentDeckSize} cards.");

        currentLoadedDeckName = newDeckName;
        PopulateSavedDecksDropdown();
    }

    private void OnDeleteDeckClicked()
    {
        if (!savedDecksDropdown.interactable || savedDecksDropdown.options[savedDecksDropdown.value].text == "No saved decks")
            return;

        string selectedDeckName = savedDecksDropdown.options[savedDecksDropdown.value].text;
        string path = Path.Combine(Application.persistentDataPath, selectedDeckName + ".json");

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Deleted deck: {selectedDeckName}");

            // Optionally: Clear deck grid if deleted deck was loaded
            if (deckNameInputField.text == selectedDeckName)
            {
                OnClearDeckClicked();
                deckNameInputField.text = "";
            }

            PopulateSavedDecksDropdown();
        }
    }

    private void OnAddToDeckClicked(CardSO card)
    {
        if (deckCards.Count(c => c.cardID == card.cardID) >= maxIdenticalCards)
        {
            Debug.LogWarning($"Cannot add more than {maxIdenticalCards} copies of {card.cardName}.");
            return;
        }
        if (deckCards.Count >= maxDeckSize)
        {
            Debug.LogWarning($"Cannot add more cards, deck is full at {maxDeckSize} cards.");
            return;
        }

        deckCards.Add(card);
        GameObject button = CreateDeckCardButton(card);
        currentDeckButtons.Add(button);
        UpdateDeckDetails();

        ShowCardDetails(card);
    }

    private void OnRemoveFromDeckClicked(string cardID)
    {
        var index = deckCards.FindIndex(c => c.cardID == cardID);
        if (index != -1)
        {
            var cardToRemove = deckCards[index];
            deckCards.RemoveAt(index);

            var buttonToRemove = currentDeckButtons
                .FirstOrDefault(b => b.GetComponent<CardButton>().cardData.cardID == cardID);

            if (buttonToRemove != null)
            {
                currentDeckButtons.Remove(buttonToRemove);
                Destroy(buttonToRemove);
            }

            bool stillInDeck = deckCards.Any(c => c.cardID == cardID);
            removeFromDeckButton.gameObject.SetActive(stillInDeck);
            UpdateDeckDetails();

            if (selectedCard != null && selectedCard.cardID == cardID)
            {
                removeFromDeckButton.onClick.RemoveAllListeners();
                if (stillInDeck)
                {
                    removeFromDeckButton.onClick.AddListener(() => OnRemoveFromDeckClicked(cardID));
                }
            }
        }
    }

    public void LoadDeckFromFile(string deckName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{deckName}.json");
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Deck file not found: {filePath}");
            return;
        }

        string json = File.ReadAllText(filePath);
        SavedDeck savedDeck = JsonUtility.FromJson<SavedDeck>(json);

        deckCards.Clear();
        foreach (var cardID in savedDeck.cardIDs)
        {
            CardSO card = cardDatabase.GetCardByID(cardID);
            if (card != null)
            {
                deckCards.Add(card);
                GameObject button = CreateDeckCardButton(card);
                currentDeckButtons.Add(button);
            }
        }

        deckNameInputField.text = savedDeck.deckName;
        UpdateDeckDetails();
        currentLoadedDeckName = savedDeck.deckName;
        Debug.Log($"Loaded deck: {savedDeck.deckName}");
    }
}
