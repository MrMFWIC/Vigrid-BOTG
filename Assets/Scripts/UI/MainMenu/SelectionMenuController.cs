using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SelectionMenuController : MonoBehaviour
{
    [Header("Deck Selection")]
    public Transform deckGridParent;
    public GameObject deckButtonPrefab;
    public TextMeshProUGUI selectedDeckNameText;
    public List<GameObject> currentDeckButtons = new List<GameObject>();
    private string selectedDeckName = null;

    [Header("Leader Selection")]
    public Transform leaderGridParent;
    public GameObject leaderButtonPrefab;
    public TextMeshProUGUI selectedLeaderNameText;
    public List<GameObject> currentLeaderButtons = new List<GameObject>();
    private string selectedLeaderName = null;

    [Header("Info Panel")]
    public GameObject infoPanel;
    public GameObject deckPrefabObj;
    public GameObject leaderPrefabObj;
    public TextMeshProUGUI infoObjName; // Name of the deck or leader
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
        confirmButton.onClick.AddListener(OnConfirmClicked);
        confirmButton.interactable = false;
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
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnDeckSelectClicked(deckName));
    }

    void OnDeckSelectClicked(string deckName)
    {
        selectedDeckName = deckName;
        selectedDeckNameText.text = $"Selected: {deckName}";
        Debug.Log($"Deck selected: {deckName}");
    }

    void PopulateLeaderGrid()
    {
        
    }

    void OnLeaderButtonClicked(string leaderName)
    {
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnLeaderSelectClicked(leaderName));
    }

    void OnLeaderSelectClicked(string leaderName)
    {
        selectedLeaderName = leaderName;
        selectedLeaderNameText.text = $"Selected: {leaderName}";
        Debug.Log($"Leader selected: {leaderName}");
    }

    void OnConfirmClicked()
    {
        //Add functionality to confirm selection
        /*
        if (deckSelected && leaderSelected)
        {
            // Proceed to the next step, e.g., start the game
            GameManager.Instance.StartGame(selectedDeckName, selectedLeaderName);
        }
        else
        {
            Debug.LogWarning("Please select both a deck and a leader before confirming.");
        }
        */
    }
    private void LoadAllLeaders()
    {
        currentLeaderButtons.ForEach(Destroy);
        currentLeaderButtons.Clear();
    }

    private void OnBackClicked()
    {
        GameManager.Instance.UIManager.HidePanel("SelectionMenu");
        GameManager.Instance.UIManager.ShowPanel("MainMenu");
    }
}
