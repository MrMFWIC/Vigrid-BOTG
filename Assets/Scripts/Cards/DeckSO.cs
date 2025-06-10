using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDeck", menuName = "Deck")]
public class DeckSO : ScriptableObject
{
    public string deckName;
    public List<CardSO> cards = new List<CardSO>();
}
