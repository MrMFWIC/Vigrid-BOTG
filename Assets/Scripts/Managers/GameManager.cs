using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public OptionsManager OptionsManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public DeckManager DeckManager { get; private set; }
    public CardPlacementManager CardPlacementManager { get; private set; }
    public CardEffectRunner CardEffectRunner { get; private set; }

    private int playerHealth;
    private int playerEssence;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();

            if (SceneManager.GetActiveScene().name == "Battlefield")
            {
                InitializeBattlefieldManagers();
            }
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        OptionsManager = GetComponentInChildren<OptionsManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        DeckManager = GetComponentInChildren<DeckManager>();

        if (OptionsManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/OptionsManager");
            if (prefab == null)
            {
                Debug.LogError($"OptionsManager prefab not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                OptionsManager = GetComponentInChildren<OptionsManager>();
            }
        }

        if (AudioManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/AudioManager");
            if (prefab == null)
            {
                Debug.LogError($"AudioManager prefab not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                AudioManager = GetComponentInChildren<AudioManager>();
            }
        }

        if (DeckManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/DeckManager");
            if (prefab == null)
            {
                Debug.LogError($"DeckManager prefab not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                DeckManager = GetComponentInChildren<DeckManager>();
            }
        }
    }

    private void InitializeBattlefieldManagers()
    {
        CardPlacementManager = GetComponentInChildren<CardPlacementManager>();
        CardEffectRunner = GetComponentInChildren<CardEffectRunner>();

        if (CardPlacementManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/CardPlacementManager");
            if (prefab == null)
            {
                Debug.LogError($"CardPlacementManager prefab not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                CardPlacementManager = GetComponentInChildren<CardPlacementManager>();
            }
        }

        if (CardEffectRunner == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/CardEffectRunner");
            if (prefab == null)
            {
                Debug.LogError($"CardEffectRunner prefab not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                CardEffectRunner = GetComponentInChildren<CardEffectRunner>();
            }
        }
    }

    public int PlayerHealth
    {
        get { return playerHealth;  }
        set { playerHealth = value; }
    }

    public int PlayerEssence
    {
        get { return playerEssence; }
        set { playerEssence = value; }
    }
}
