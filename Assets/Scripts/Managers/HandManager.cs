using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    public GameObject cardPrefab;
    public Transform handTransform;
    public float fanSpread = -7.5f;
    public float horizontalSpacing = 120f;
    public float verticalSpacing = 60f;
    public int maxHandSize = 8;
    public List<GameObject> cardsInHand = new List<GameObject>();

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

    void Start()
    {
        Debug.Log("HandManager Start called.");
        if (SceneManager.GetActiveScene().name != "Battlefield")
        {
            return;
        }

        Debug.Log("HandManager started. Initializing hand visuals...");
        GameObject handPositionObj = GameObject.Find("HandPosition");
        if (handPositionObj == null)
        {
            Debug.Log($"Hand Position Object not found in scene");
        }
        else
        {
            handTransform = handPositionObj.transform;
            Debug.Log($"Hand Position Object found: {handTransform.name}");
        }
    }

    void Update()
    {
        //UpdateHandVisuals();
    }

    public void AddCardToHand(CardSO cardData)
    {
        if (handTransform == null)
        {
            Debug.LogWarning("Hand Transform is not set. Calling Start function.");
            Start();
            if (handTransform == null)
            {
                Debug.LogWarning("Hand Transform is still not set after Start. Cannot add card to hand.");
                return;
            }
        }

        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);
        newCard.GetComponent<CardDisplay>().cardInstance = cardData;
        UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (horizontalSpacing * (i - (cardCount - 1) / 2f));
            float normalizedPosition = (2f * i / (cardCount - 1) - 1f);
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
