using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPlacementCell : MonoBehaviour
{
    public int cellIndex;
    public bool isOccupied = false;
    public GameObject cardInCell;
    public RectTransform cardRectTransform;

    public void PlaceCard(GameObject card)
    {
        GameObject newObj = Instantiate(card, transform.position, Quaternion.identity);

        if (newObj != null )
        {
            Destroy(card);
        }
        else
        {
            Debug.Log($"Failed to instantiate card at index {cellIndex}.");
            return;
        }

        newObj.transform.SetParent(transform);
        cardInCell = newObj;
        isOccupied = true;

        /*cardRectTransform = card.GetComponent<RectTransform>();
        cardInCell = card;
        cardRectTransform.SetParent(gameObject.GetComponent<RectTransform>());
        cardRectTransform.localPosition = Vector3.zero;
        isOccupied = true;*/
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
