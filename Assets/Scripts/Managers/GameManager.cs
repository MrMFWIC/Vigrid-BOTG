using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    public OptionsManager OptionsManager => OptionsManager.Instance;
    public AudioManager AudioManager => AudioManager.Instance;
    public DeckManager DeckManager => DeckManager.Instance;
    public UIManager UIManager => UIManager.Instance;
    public TextSizeManager TextSizeManager => TextSizeManager.Instance;
    public GraphicsSettingsManager GraphicsSettingsManager => GraphicsSettingsManager.Instance;
    public CardPlacementManager CardPlacementManager => CardPlacementManager.Instance;
    public HandManager HandManager => HandManager.Instance;  

    private CardEffectRunner _cardEffectRunner;
    public CardEffectRunner CardEffectRunner => _cardEffectRunner;


    [Header("Battle Details")]
    public CardDatabase cardDatabase;
    public SavedDeck selectedDeck;
    public LeaderSO selectedLeader;

    [Header("Player(s) Details")]
    private int playerHealth;
    private int playerEssence;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Only the true singleton subscribes
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.LogWarning("[GameManager] Subscribed to sceneLoaded.");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cardDatabase = Resources.Load<CardDatabase>("CardDatabase");
        Debug.Log(cardDatabase == null ?
                               "CardDatabase not found in Resources." :
                               "CardDatabase loaded successfully.");

        if (scene.name == "Battlefield")
        {
            InitializeBattlefieldManagers();
            DeckManager.InitializeDeck();
            CardPlacementManager.BeginInitialization();
        }
    }

    private void InitializeBattlefieldManagers()
    {
        _cardEffectRunner = LoadOrInstantiateManager(ref _cardEffectRunner, "CardEffectRunner");
    }

    private T LoadOrInstantiateManager<T>(ref T managerRef, string prefabName) where T : Component
    {
        managerRef = GetComponentInChildren<T>();

        if (managerRef != null)
        {
            Debug.Log($"{prefabName} found in children.");
        }
        else if (managerRef == null)
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");
            Debug.Log(prefab == null ?
                               $"Prefab '{prefabName}' not found in Resources/Prefabs" :
                               $"Prefab '{prefabName}' loaded, instantiating now.");
            if (prefab == null)
            {
                Debug.LogError($"{prefabName} prefab not found.");
                return null;
            }
            else
            {
                GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity, transform);
                managerRef = instance.GetComponent<T>();
                Debug.Log(managerRef == null ?
                                  $"{prefabName} was instantiated but has no {typeof(T).Name} component!" :
                                  $"{prefabName} instantiated successfully.");
            }
        }

        return managerRef;
    }

    public void QuitGame()
    {
        Application.Quit();
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
