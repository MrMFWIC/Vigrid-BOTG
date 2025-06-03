using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPlacementCell : MonoBehaviour
{
    public int cellIndex;
    public bool isOccupied = false;
    public GameObject cardInCell;
    public GameObject highlight;
    public RectTransform cardRectTransform;
    public CardPlacementManager cardPlacementManager;

    private void Start()
    {
        cardPlacementManager = FindFirstObjectByType<CardPlacementManager>();
    }

    public void HighlightSlot(bool state, GameObject card)
    {
        if (isOccupied) return;

        if (highlight != null)
        {
            highlight.SetActive(state);

            if (!cardPlacementManager.IsValidSlot(this.cellIndex, card))
            {
                highlight.GetComponent<Image>().color = new Color(1, 0, 0, 0.75f); // Red for invalid slot
            }
            else
            {
                highlight.GetComponent<Image>().color = new Color(0, 1, 0, 0.75f); // Green for valid slot
            }
        }
    }

    public void PlaceCard(GameObject card)
    {
        GameObject newObj = Instantiate(card, transform.position, Quaternion.identity);
        newObj.transform.SetParent(transform);
        if (cellIndex == 0)
        {
            newObj.transform.localScale = new Vector3(1, 1.4f, 1);
        }
        else
        {
            newObj.transform.localScale = Vector3.one;
        }

        if (newObj != null )
        {
            Destroy(card);
            newObj.GetComponent<CardMovement>().DisableInteractions();
            newObj.GetComponent<CardMovement>().enabled = false;
            foreach (var graphic in newObj.GetComponentsInChildren<Graphic>())
            {
                graphic.raycastTarget = false;
            }
        }
        else
        {
            Debug.Log($"Failed to instantiate card at index {cellIndex}.");
            return;
        }

        cardInCell = newObj;
        isOccupied = true;
        Debug.Log("Card parent after placement: " + newObj.transform.parent.name);
    }

    public void RemoveCard()
    {
        if (isOccupied)
        {
            Destroy(cardInCell);
            cardInCell = null;
            isOccupied = false;
        }
        else
        {
            Debug.LogWarning("Cell is already empty!");
        }
    }
}
