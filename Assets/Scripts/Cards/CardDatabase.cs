using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Cards/Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<CardEntry> allCards = new List<CardEntry>();

    [System.Serializable]
    public class CardEntry
    {
        public CardSO card;
    }

    public CardSO GetCardByID(string cardID)
    {
        return allCards
            .FirstOrDefault(entry => entry.card != null && entry.card.cardID == cardID)
            ?.card;
    }
}
