using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public List<UIPanel> panels;

    private Dictionary<string, CanvasGroup> panelDict = new();
    private static bool sceneLoadedSubscribed = false;

    void Awake()
    {
        Debug.Log("UIManager Awake called.");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!sceneLoadedSubscribed)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            sceneLoadedSubscribed = true;
        }
      
        //InitializePanelsInScene();
    }

    private void OnDestroy()
    {
        if (sceneLoadedSubscribed)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            sceneLoadedSubscribed = false;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializePanelsInScene();

        if (scene.name == "MainMenu")
        {
            ShowPanel("MainMenu");
        }
    }

    private void InitializePanelsInScene()
    {
        panelDict.Clear();

        UIPanel[] scenePanels = FindObjectsByType<UIPanel>(FindObjectsSortMode.None);
        foreach (var panel in scenePanels)
        {
            if (panel != null && panel.canvasGroup != null && !panelDict.ContainsKey(panel.panelName))
            {
                panelDict.Add(panel.panelName, panel.canvasGroup);
            }
        }

        HideAllPanels();
    }

    public void HideAllPanels()
    {
        foreach (var panel in panelDict)
        {
            HidePanel(panel.Key);
        }
    }

    public void ShowPanel(string name)
    {
        if (panelDict.TryGetValue(name, out var canvasGroup))
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void HidePanel(string name)
    {
        if (panelDict.TryGetValue(name, out var canvasGroup))
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void TogglePanel(string name)
    {
        if (panelDict.TryGetValue(name, out var canvasGroup))
        {
            bool isActive = canvasGroup.alpha > 0.5f;
            if (isActive)
                HidePanel(name);
            else
                ShowPanel(name);
        }
    }

    public void FadePanel(string name, float targetAlpha, float duration)
    {
        if (panelDict.TryGetValue(name, out var canvasGroup))
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, targetAlpha, duration));
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float target, float duration)
    {
        float start = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = target;
        canvasGroup.interactable = target > 0.5f;
        canvasGroup.blocksRaycasts = target > 0.5f;
    }
}
