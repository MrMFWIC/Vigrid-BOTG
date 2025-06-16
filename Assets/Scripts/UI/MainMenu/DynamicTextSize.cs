using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DynamicTextSize : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        ApplyTextSize();
    }

    public void ApplyTextSize()
    {
        text.fontSize = GameManager.Instance.TextSizeManager.GetFontSize();
    }
}
