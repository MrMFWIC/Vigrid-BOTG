using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuController : MonoBehaviour
{
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnBackClicked()
    {
        GameManager.Instance.UIManager.HidePanel("ShopMenu");
        GameManager.Instance.UIManager.ShowPanel("MainMenu");
    }
}
