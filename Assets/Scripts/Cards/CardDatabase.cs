using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Cards/Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<CardEntry> allCards = new List<CardEntry>();

    [System.Serializable]
    public class CardEntry
    {
        public string guid;
        public CardSO card;
    }

    public CardSO GetCardByGUID(string guid)
    {
        return allCards.Find(entry => entry.guid == guid)?.card;
    }
}
