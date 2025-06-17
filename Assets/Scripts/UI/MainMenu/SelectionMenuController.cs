using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class SelectionMenuController : MonoBehaviour
{
    [Header("Deck Selection")]
    public Transform deckGridParent;
    public GameObject deckButtonPrefab;
    public TextMeshProUGUI selectedDeckNameText;
    public List<GameObject> currentDeckButtons = new List<GameObject>();
    private SavedDeck selectedDeck = null;
    private string selectedDeckName = null;

    [Header("Leader Selection")]
    public Transform leaderGridParent;
    public GameObject leaderButtonPrefab;
    public TextMeshProUGUI selectedLeaderNameText;
    public List<GameObject> currentLeaderButtons = new List<GameObject>();
    private LeaderSO selectedLeader = null;
    private string selectedLeaderName = null;

    [Header("Info Panel")]
    public GameObject infoPanel;
    public GameObject deckPrefabObj;
    public Sprite backupDeckImage;
    public GameObject leaderPrefabObj;
    public TextMeshProUGUI infoObjDescription; // Deck details or leader description
    public Button selectButton; //Add different listener based on if deck or leader is selected


    [Header("Misc")]
    public Button backButton;
    public Button confirmButton;
    public bool deckSelected = false;
    public bool leaderSelected = false;

    private void Start()
    {
        PopulateDeckGrid();
        PopulateLeaderGrid();
        infoPanel.SetActive(false);
        backButton.onClick.AddListener(OnBackClicked);
    }

    void Update()
    {
        confirmButton.gameObject.SetActive(deckSelected && leaderSelected);
        if (confirmButton.interactable)
        {
            confirmButton.onClick.AddListener(() => OnConfirmClicked(selectedLeader, selectedDeck));
        }
    }

    void PopulateDeckGrid()
    {
        foreach (var button in currentDeckButtons)
        {
            Destroy(button);
        }
        currentDeckButtons.Clear();

        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");

        foreach (string filePath in files)
        {
            string deckName = Path.GetFileNameWithoutExtension(filePath);
            string json = File.ReadAllText(filePath);
            SavedDeck savedDeck = JsonUtility.FromJson<SavedDeck>(json);

            GameObject buttonGO = Instantiate(deckButtonPrefab, deckGridParent);
            DeckButton deckButton = buttonGO.GetComponent<DeckButton>();
            if (deckButton != null)
            {
                deckButton.Setup(deckName);
            }

            buttonGO.GetComponent<Button>().onClick.AddListener(() => OnDeckButtonClicked(deckName));
            currentDeckButtons.Add(buttonGO);
        }

        // Add premade decks (optional, stubbed for later)
        // AddPremadeDeck("StarterDeck1", 30);
    }

    void OnDeckButtonClicked(string deckName)
    {
        infoPanel.SetActive(true);
        deckPrefabObj.SetActive(true);
        leaderPrefabObj.SetActive(false);

        SavedDeck savedDeck = null;

        string path = Path.Combine(Application.persistentDataPath, deckName + ".json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            savedDeck = JsonUtility.FromJson<SavedDeck>(json);

            int totalCards = savedDeck.cardIDs.Count;

            List<CardSO> cards = new List<CardSO>();
            foreach (var id in savedDeck.cardIDs)
            {
                CardSO card = GameManager.Instance.CardDatabase.GetCardByID(id);
                if (card != null)
                    cards.Add(card);
                else
                    Debug.LogWarning($"Card ID {id} not found in database.");
            }

            int unitCount = cards.Count(c => c.cardType != CardSO.CardType.Spell);
            int spellCount = cards.Count(c => c.cardType == CardSO.CardType.Spell);
            float averageCost = 0;
            if (cards.Count > 0)
            {
                averageCost = cards.Count > 0 ? (int)cards.Average(card => card.cardCost) : 0;
            }

            infoObjDescription.text = $"Total Cards: {totalCards}\n" +
                                      $"Units: {unitCount}\n" +
                                      $"Spells: {spellCount}\n" +
                                      $"Average Cost: {averageCost:F1}\n"; // Format with 1 decimal
        }
        else
        {
            infoObjDescription.text = "Deck data not found.";
        }

        if (savedDeck.deckImage == null)
        {
            deckPrefabObj.GetComponent<DeckButton>().deckImage.sprite = backupDeckImage;
        }
        else
        {
            deckPrefabObj.GetComponent<DeckButton>().deckImage.sprite = savedDeck.deckImage;
        }

        deckPrefabObj.GetComponent<DeckButton>().Setup(deckName);
        deckPrefabObj.GetComponent<DeckButton>().SetSelected(true);
        deckPrefabObj.GetComponent<Button>().interactable = false;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnDeckSelectClicked(savedDeck));
    }

    void OnDeckSelectClicked(SavedDeck deck)
    {
        selectedDeck = deck;
        selectedDeckName = deck.deckName;
        deckSelected = true;
        selectedDeckNameText.text = $"Selected: {deck.deckName}";
        Debug.Log($"Deck selected: {deck.deckName}");
    }

    void PopulateLeaderGrid()
    {
        foreach (var button in currentLeaderButtons)
        {
            Destroy(button);
        }
        currentLeaderButtons.Clear();

        LeaderSO[] allLeaders = Resources.LoadAll<LeaderSO>("Leaders");

        foreach (LeaderSO leader in allLeaders)
        {
            GameObject buttonGO = Instantiate(leaderButtonPrefab, leaderGridParent);
            LeaderButton leaderButton = buttonGO.GetComponent<LeaderButton>();
            if (leaderButton != null)
            {
                leaderButton.Setup(leader);
            }
            buttonGO.GetComponent<Button>().onClick.AddListener(() => OnLeaderButtonClicked(leader));
            currentLeaderButtons.Add(buttonGO);
        }
    }

    void OnLeaderButtonClicked(LeaderSO leader)
    {
        infoPanel.SetActive(true);
        deckPrefabObj.SetActive(false);
        leaderPrefabObj.SetActive(true);
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnLeaderSelectClicked(leader));

        if (leader != null)
        {
            leaderPrefabObj.GetComponent<LeaderButton>().Setup(leader);
            leaderPrefabObj.GetComponent<LeaderButton>().SetSelected(true);
            leaderPrefabObj.GetComponent<Button>().interactable = false;
            infoObjDescription.text = leader.leaderEffect;
        }
        else
        {
            infoObjDescription.text = "Leader data not found.";
        }
    }

    void OnLeaderSelectClicked(LeaderSO leader)
    {
        selectedLeader = leader;
        selectedLeaderName = leader.leaderName;
        leaderSelected = true;
        selectedLeaderNameText.text = $"Selected: {leader.leaderName}";
        Debug.Log($"Leader selected: {leader.leaderName}");
    }

    void OnConfirmClicked(LeaderSO leader, SavedDeck deck)
    {
        GameManager.Instance.selectedDeck = deck;
        Debug.Log($"GameManager Selected Deck: {GameManager.Instance.selectedDeck}");
        GameManager.Instance.selectedLeader = leader;
        Debug.Log($"GameManager Selected Leader: {GameManager.Instance.selectedLeader}");

        // Load the battlefield scene or start the game logic
    }

    private void OnBackClicked()
    {
        GameManager.Instance.UIManager.HidePanel("SelectionMenu");
        GameManager.Instance.UIManager.ShowPanel("MainMenu");
    }
}
