using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckManager : MonoBehaviour
{
    private HandManager handManager;
    public List<CardSO> allCards = new List<CardSO>();
    private int currentIndex = 0;
    public int startingHandSize = 5;
    public int maxHandSize;
    public int currentHandSize;

    void Start()
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

    private void Update()
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
}
