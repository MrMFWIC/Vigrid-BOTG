using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button startGameButton;
    public Button settingsButton;
    public Button archivesButton;
    public Button quitButton;
    public Button shopButton;

    private void Start()
    {
        startGameButton.onClick.AddListener(OnStartGameClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        archivesButton.onClick.AddListener(OnArchivesClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        shopButton.onClick.AddListener(OnShopClicked);
    }

    private void OnStartGameClicked()
    {
        GameManager.Instance.UIManager.HidePanel("MainMenu");
        GameManager.Instance.UIManager.ShowPanel("SelectionMenu");
    }

    private void OnSettingsClicked()
    {
        // Open settings menu
        GameManager.Instance.UIManager.HidePanel("MainMenu");
        GameManager.Instance.UIManager.ShowPanel("SettingsMenu");
    }

    private void OnArchivesClicked()
    {
        // Open archives menu
        GameManager.Instance.UIManager.HidePanel("MainMenu");
        GameManager.Instance.UIManager.ShowPanel("ArchivesMenu");
    }


    private void OnShopClicked()
    {
        // Open shop menu
        GameManager.Instance.UIManager.HidePanel("MainMenu");
        GameManager.Instance.UIManager.ShowPanel("ShopMenu");
    }

    private void OnQuitClicked()
    {
        // Quit the application
        GameManager.Instance.QuitGame();
    }
}