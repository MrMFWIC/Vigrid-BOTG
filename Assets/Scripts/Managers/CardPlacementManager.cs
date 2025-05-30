using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;

public class CardPlacementManager : MonoBehaviour
{
    public CardPlacementCell[] playerSlots;
    public CardPlacementCell[] enemySlots;

    public GameObject playerSlotsParent;
    public GameObject enemySlotsParent;

    private void Awake()
    {
        GameObject canvas = GameObject.FindFirstObjectByType<Canvas>().gameObject;

        if (canvas != null)
        {
            playerSlotsParent = canvas.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PlayerSlots")?.gameObject;
            enemySlotsParent = canvas.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnemySlots")?.gameObject;

            if (playerSlotsParent == null || enemySlotsParent == null)
            {
                Debug.LogError("Parent objects not found in the canvas");
                return;
            }
        }
        else
        {
            Debug.LogError("Canvas not found in the scene");
            return;
        }

        FillSlotsArrays();
    }

    private void FillSlotsArrays()
    {
        if (playerSlotsParent != null)
        {
            int count = playerSlotsParent.transform.childCount;
            playerSlots = new CardPlacementCell[count];

            for (int i = 0; i < count; i++)
            {
                Transform child = playerSlotsParent.transform.GetChild(i);
                CardPlacementCell cell = child.GetComponent<CardPlacementCell>();

                if (cell != null)
                {
                    cell.cellIndex = i; 
                    playerSlots[i] = cell;
                }
            }
        }

        if (enemySlotsParent != null)
        {
            int count = enemySlotsParent.transform.childCount;
            enemySlots = new CardPlacementCell[count];

            for (int i = 0; i < count; i++)
            {
                Transform child = enemySlotsParent.transform.GetChild(i);
                CardPlacementCell cell = child.GetComponent<CardPlacementCell>();

                if (cell != null)
                {
                    cell.cellIndex = i;
                    enemySlots[i] = cell;
                }
            }
        }
    }

    public bool IsValidSlot(int index, GameObject card)
    {
        CardSO cardData = card.GetComponent<CardDisplay>().cardInstance;

        if (playerSlots[index].isOccupied)
        {
            Debug.Log("Slot is already occupied. Cannot place card here.");
            return false;
        }
        if (cardData == null)
        {
            Debug.Log("Card data is missing on the card object.");
            return false;
        }
        if (index < 0 || index >= playerSlots.Length)
        {
            Debug.Log("Invalid slot index for player slots.");
            return false;
        }
        if (index == 0 && cardData.cardType != CardSO.CardType.Spell)
        {
            Debug.Log("Only Spell cards may be played in the spell slot.");
            return false;
        }
        if (index > 0 && index < playerSlots.Length && cardData.cardType != CardSO.CardType.Unit)
        {
            Debug.Log("Spell cards must be played in the spell slot.");
            return false;
        }

        return true;
    }

    public void PlaceCardInPlayerSlot(int index, GameObject card)
    {
        CardSO cardData = card.GetComponent<CardDisplay>().cardInstance;
        playerSlots[index].PlaceCard(card);
        card.GetComponent<CardMovement>().enabled = false;
    }

    public void PlaceCardInEnemySlot(int index, GameObject card)
    {
        //The following will be code used to place cards in enemy slots
        //Will be dependent on the networking logic and how the game handles enemy cards
        //Temp Example for now
        /*if (index < 0 || index >= enemySlots.Length)
        {
            Debug.LogError("Invalid slot index for enemy slots.");
            return;
        }
        if (enemySlots[index].isOccupied)
        {
            Debug.LogWarning("Slot is already occupied. Replacing existing card.");
            enemySlots[index].RemoveCard();
        }
        enemySlots[index].PlaceCard(card);*/
    }
}
