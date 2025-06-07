using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public List<UIPanel> panels;

    private Dictionary<string, CanvasGroup> panelDict;

    void Awake()
    {
        panelDict = new Dictionary<string, CanvasGroup>();

        foreach (var panel in panels)
        {
            if (!panelDict.ContainsKey(panel.panelName))
                panelDict.Add(panel.panelName, panel.canvasGroup);
        }

        HideAllPanels();

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            ShowPanel("MainMenu");
        }
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
