using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public static GameManager Instance { get; private set; }

    private OptionsManager _optionsManager;
    public OptionsManager OptionsManager => _optionsManager;

    private AudioManager _audioManager;
    public AudioManager AudioManager => _audioManager;

    private DeckManager _deckManager;
    public DeckManager DeckManager => _deckManager;
    
    private UIManager _uiManager;
    public UIManager UIManager => _uiManager;
    
    private TextSizeManager _textSizeManager;
    public TextSizeManager TextSizeManager => _textSizeManager;
    
    private GraphicsSettingsManager _graphicsSettingsManager;
    public GraphicsSettingsManager GraphicsSettingsManager => _graphicsSettingsManager;
    
    private CardPlacementManager _cardPlacementManager;
    public CardPlacementManager CardPlacementManager => _cardPlacementManager;
    
    private CardEffectRunner _cardEffectRunner;
    public CardEffectRunner CardEffectRunner => _cardEffectRunner;

    private HandManager _handManager;
    public HandManager HandManager => _handManager;

    [Header("Battle Details")]
    public CardDatabase CardDatabase;
    public SavedDeck selectedDeck;
    public LeaderSO selectedLeader;

    [Header("Player(s) Details")]
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
        _uiManager = LoadOrInstantiateManager(ref _uiManager, "UIManager");
        _audioManager = LoadOrInstantiateManager(ref _audioManager, "AudioManager");
        _deckManager = LoadOrInstantiateManager(ref _deckManager, "DeckManager");
        _handManager = LoadOrInstantiateManager(ref _handManager, "HandManager");
        _textSizeManager = LoadOrInstantiateManager(ref _textSizeManager, "TextSizeManager");
        _graphicsSettingsManager = LoadOrInstantiateManager(ref _graphicsSettingsManager, "GraphicsSettingsManager");
        _optionsManager = LoadOrInstantiateManager(ref _optionsManager, "OptionsManager");
    }

    private void InitializeBattlefieldManagers()
    {
        _cardPlacementManager = LoadOrInstantiateManager(ref _cardPlacementManager, "CardPlacementManager");
        _cardEffectRunner = LoadOrInstantiateManager(ref _cardEffectRunner, "CardEffectRunner");
    }

    private T LoadOrInstantiateManager<T>(ref T managerRef, string prefabName) where T : Component
    {
        managerRef = GetComponentInChildren<T>();

        if (managerRef == null)
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");
            if (prefab == null)
            {
                Debug.LogError($"{prefabName} prefab not found.");
            }
            else
            {
                GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity, transform);
                managerRef = instance.GetComponent<T>();

                if (managerRef == null)
                {
                    Debug.LogError($"{prefabName} component missing on prefab.");
                }
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
