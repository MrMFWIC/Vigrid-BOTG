using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CardPlacementManager cardPlacementManager;
    private HandManager handmanager;

    private RectTransform rectTransform;
    private Canvas canvas;
    private GraphicRaycaster uiRaycaster;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private int currentState = 0;
    private bool isPlayed = false;

    [SerializeField] private float selectScale = 1.2f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.2f;

    void Awake()
    {
        cardPlacementManager = FindFirstObjectByType<CardPlacementManager>();
        handmanager = FindFirstObjectByType<HandManager>();
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;
    }

    void Start()
    {
        StartCoroutine(DelayedCanvasInit());
    }

    void Update()
    {
        if (isPlayed) return;

        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDragState();
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();
                break;
        }
    }

    public void DisableInteractions()
    {
        isPlayed = true;
        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }

    public void EnableInteractions(GameObject card)
    {
        isPlayed = false;
        foreach (var graphic in card.GetComponentsInChildren<Graphic>())
        {
            graphic.raycastTarget = true;
        }
        TransitionToState0();
    }

    private void TransitionToState0()
    {
        currentState = 0;
        rectTransform.localScale = originalScale;
        rectTransform.localPosition = originalPosition;
        rectTransform.localRotation = originalRotation;
        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalScale = rectTransform.localScale;
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;

            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
            originalPanelLocalPosition = rectTransform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpFactor);

                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = 3;
                    rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor);
                }
            }
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState()
    {
        rectTransform.localPosition = playPosition;
        rectTransform.localRotation = Quaternion.identity;

        if (!playArrow.activeSelf)
            playArrow.SetActive(true);

        if (Input.GetMouseButtonUp(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(pointerData, results);
            CardPlacementCell nearestCell = null;
            float nearestDistance = float.MaxValue;

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("CardSlot"))
                {
                    Debug.Log("Raycast hit: " + result.gameObject.name);
                    CardPlacementCell cell = result.gameObject.GetComponentInParent<CardPlacementCell>();

                    if (cell != null)
                    {
                        float distance = Vector2.Distance(result.screenPosition, Input.mousePosition);

                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestCell = cell;
                        }
                    }
                }
            }

            if (nearestCell != null)
            {
                Debug.Log("Best CardPlacementCell found with index: " + nearestCell.cellIndex);
                int targetPos = nearestCell.cellIndex;

                if (cardPlacementManager.IsValidSlot(targetPos, gameObject))
                {
                    cardPlacementManager.PlaceCardInPlayerSlot(targetPos, gameObject);
                    handmanager.cardsInHand.Remove(gameObject);
                    handmanager.UpdateHandVisuals();
                }
            }
            else
            {
                Debug.Log("CardPlacementCell component not found.");
            }

            TransitionToState0();
        }
    }

    private IEnumerator DelayedCanvasInit()
    {
        yield return null; // Wait for the next frame to ensure the canvas is initialized
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in hierarchy for card: " + gameObject.name);
        }
        else
        {
            uiRaycaster = canvas.GetComponent<GraphicRaycaster>();
        }
    }
}
