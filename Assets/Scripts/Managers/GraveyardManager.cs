using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GraveyardManager : MonoBehaviour
{
    public static GraveyardManager Instance { get; private set; }

    public GraveyardCell playerGraveyardCell;
    public GraveyardCell enemyGraveyardCell;

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

    private void InitializeGraveyardManager()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("BattlefieldCanvas");

        if (canvas != null)
        {
            GameObject playerGYObj = FindChildByName(canvas.transform, "PlayerGraveyardCell");
            GameObject enemyGYObj = FindChildByName(canvas.transform, "EnemyGraveyardCell");

            if (playerGYObj == null || enemyGYObj == null)
            {
                Debug.LogError("Parent objects not found in the canvas");
                return;
            }
            else
            {
                playerGraveyardCell = playerGYObj.GetComponent<GraveyardCell>();
                enemyGraveyardCell = enemyGYObj.GetComponent<GraveyardCell>();
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

    public void PlaceCardInPlayerGraveyard(GameObject card)
    {
        playerGraveyardCell.PlaceCard(card);
    }

    public void PlaceCardInEnemyGraveyard(GameObject card)
    {
        //Implementation here
    }

    IEnumerator DelayedInitialize()
    {
        yield return null; // wait one frame
        InitializeGraveyardManager();
    }
}
