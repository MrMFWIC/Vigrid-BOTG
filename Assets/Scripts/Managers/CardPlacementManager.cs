using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardPlacementManager : MonoBehaviour
{
    public static CardPlacementManager Instance { get; private set; }

    public CardPlacementCell[] playerSlots;
    public CardPlacementCell[] enemySlots;

    public GameObject playerSlotsParent;
    public GameObject enemySlotsParent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void BeginInitialization()
    {
        StartCoroutine(DelayedInitialize());
    }

    private void InitializeCardPlacementManager()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("BattlefieldCanvas");

        if (canvas != null)
        {
            playerSlotsParent = FindChildByName(canvas.transform, "PlayerSlots");
            enemySlotsParent = FindChildByName(canvas.transform, "EnemySlots");

            if (playerSlotsParent == null || enemySlotsParent == null)
            {
                Debug.LogError("Parent objects not found in the canvas");
                return;
            }
            else
            {
                FillSlotsArrays();
            }
        }
        else
        {
            Debug.LogError("Canvas not found in the scene");
            return;
        }
    }

    private GameObject FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }

        return null;
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
        card.GetComponent<CardMovement>().DisableInteractions();
        card.GetComponent<CardMovement>().enabled = false;
        foreach (var graphic in card.GetComponentsInChildren<Graphic>())
        {
            graphic.raycastTarget = false;
        }
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

    IEnumerator DelayedInitialize()
    {
        yield return null; // wait one frame
        InitializeCardPlacementManager();
    }
}
