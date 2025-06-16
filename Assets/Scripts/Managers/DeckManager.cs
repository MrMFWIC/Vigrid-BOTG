using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckManager : MonoBehaviour
{
    private HandManager handManager;
    private SavedDeck selectedDeck;
    private CardDatabase cardDatabase;
    private List<string> shuffledDeck = new List<string>();
    private int currentIndex = 0;
    public int startingHandSize = 5;
    public int maxHandSize;
    public int currentHandSize;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Battlefield")
        {
            return;
        }

        cardDatabase = GameManager.Instance.CardDatabase;
        if (cardDatabase == null)
        {
            Debug.LogError("CardDatabase is not assigned in GameManager. Please assign it in the inspector.");
            return;
        }

        handManager = GameManager.Instance.HandManager;
        if (handManager == null)
        {
            Debug.LogError("HandManager is not assigned in GameManager.");
            return;
        }
        maxHandSize = handManager.maxHandSize;

        LoadSelectedDeck();
        if (selectedDeck == null || selectedDeck.cardIDs == null || selectedDeck.cardIDs.Count == 0)
        {
            Debug.LogError("Selected deck is null or empty. Please assign a valid DeckSO.");
            return;
        }

        if (!ValidateDeckCardIDs())
        {
            Debug.LogError("Deck validation failed. Please check the card IDs in the selected deck.");
            return;
        }

        shuffledDeck = new List<string>(selectedDeck.cardIDs);
        ShuffleDeck();

        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard();
        }
    }

    private void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard();
        }
    }

    private void LoadSelectedDeck()
    {
        string selectedDeckName = GameManager.Instance.selectedDeck.deckName;
        if (string.IsNullOrEmpty(selectedDeckName))
        {
            Debug.LogError("No deck selected. Please select a deck before starting the game.");
            return;
        }

        string path = Path.Combine(Application.persistentDataPath, selectedDeckName + ".json");
        if (!File.Exists(path))
        {
            Debug.LogError($"Deck file not founc: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        selectedDeck = JsonUtility.FromJson<SavedDeck>(json);
    }

    private bool ValidateDeckCardIDs()
    {
        List<string> missingIDs = new List<string>();

        foreach (string cardID in selectedDeck.cardIDs)
        {
            if (cardDatabase.GetCardByGUID(cardID) == null)
            {
                missingIDs.Add(cardID);
            }
        }

        if (missingIDs.Count > 0)
        {
            Debug.LogError($"Deck validation failed. The following card IDs are missing from the CardDatabase:\n{string.Join(", ", missingIDs)}");
            return false;
        }

        Debug.Log("Deck validation passed. All card IDs are valid.");
        return true;
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledDeck.Count);
            (shuffledDeck[i], shuffledDeck[randomIndex]) = (shuffledDeck[randomIndex], shuffledDeck[i]);
        }
    }

    public void DrawCard()
    {
        if (currentIndex >= shuffledDeck.Count || handManager.cardsInHand.Count >= maxHandSize)
        {
            Debug.Log("No more cards to draw or hand is full.");
            return;
        }

        string cardID = shuffledDeck[currentIndex];
        currentIndex++;

        CardSO cardData = cardDatabase.GetCardByGUID(cardID);
        if (cardData != null)
        {
            handManager.AddCardToHand(cardData);
        }
        else
        {
            Debug.LogError($"Card with GUID: {cardID} not found in CardDatabase.");
        }
    }
}







/*public class DeckManager : MonoBehaviour
{
    private HandManager handManager;
    public List<CardSO> allCards = new List<CardSO>();
    private int currentIndex = 0;
    public int startingHandSize = 5;
    public int maxHandSize;
    public int currentHandSize;

    void /Start()
    {
        if (SceneManager.GetActiveScene().name != "Battlefield")
        {
            return; // Only initialize in the Battlefield scene
        }

        CardSO[] cards = Resources.LoadAll<CardSO>("Cards");
        allCards.AddRange(cards);
        handManager = FindFirstObjectByType<HandManager>();
        maxHandSize = handManager.maxHandSize;

        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard(handManager);
        }
    }

    private void /Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0)
        {
            return;
        }

        if (currentHandSize < maxHandSize)
        {
            CardSO nextCard = allCards[currentIndex];
            handManager.AddCardToHand(nextCard);
            currentIndex = (currentIndex + 1) % allCards.Count;
        }
    }
}*/
