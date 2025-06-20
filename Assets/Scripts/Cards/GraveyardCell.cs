using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraveyardCell : MonoBehaviour
{
    public GraveyardManager graveyardManager;

    [Header("UI/Visual References")]
    public GameObject highlight;
    public RectTransform cardRectTransform;
    public Transform stackParent; // An empty RectTransform to hold stacked cards
    public GameObject cardPrefab; // Reference to your card prefab (display only)
    public TextMeshProUGUI graveyardCountText;

    [Header("Settings")]
    private float cardStackOffsetY = -0.2f;
    private float cardScale = 0.65f;
    private float maxStackHeight = 10f;

    [Header("Runtime")]
    public List<CardSO> cardsInGraveyard = new List<CardSO>();
    private List<GameObject> stackVisuals = new List<GameObject>();

    void Start()
    {
        graveyardManager = GameManager.Instance.GraveyardManager;
    }

    public void HighlightSlot(bool state, GameObject card)
    {
        if (highlight != null)
            highlight.SetActive(state);
    }

    public void PlaceCard(GameObject card)
    {
        // Store data
        CardSO cardData = card.GetComponent<CardDisplay>().cardInstance;
        cardsInGraveyard.Add(cardData);

        // Destroy the card object
        Destroy(card);

        // Update the visual stack
        UpdateGraveyardVisual(cardData);
    }

    private void UpdateGraveyardVisual(CardSO newCardData)
    {
        // Update count text
        if (graveyardCountText != null)
            graveyardCountText.text = cardsInGraveyard.Count.ToString();

        // Instantiate new visual card
        GameObject newCardVisual = Instantiate(cardPrefab, stackParent);
        newCardVisual.GetComponent<CardDisplay>().cardInstance = newCardData;

        foreach (var graphic in newCardVisual.GetComponentsInChildren<Graphic>())
            graphic.raycastTarget = false;

        if (newCardVisual.TryGetComponent(out CardMovement movement))
            movement.enabled = false;

        stackVisuals.Add(newCardVisual);

        // Calculate dynamic Y offset based on stack height constraint
        int cardCount = stackVisuals.Count;

        float actualOffsetY = cardCount > 1
            ? Mathf.Min(cardStackOffsetY, maxStackHeight / (cardCount - 1))
            : 0f;

        // Reposition all cards with shrinking Y and staggered Z
        for (int i = 0; i < stackVisuals.Count; i++)
        {
            RectTransform rt = stackVisuals[i].GetComponent<RectTransform>();

            // Apply vertical offset
            rt.anchoredPosition = new Vector2(0, i * actualOffsetY);

            // Optional: Set Z index via SetSiblingIndex to keep draw order
            stackVisuals[i].transform.SetSiblingIndex(i);

            // Apply scaling
            rt.localScale = new Vector3(cardScale, cardScale, 1f);
        }
    }

    public void RemoveCard()
    {
        
    }
}
