using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckVisualController : MonoBehaviour
{
    public static DeckVisualController Instance { get; private set; }

    public TextMeshProUGUI deckCountText;
    public List<GameObject> deckStackImages;
    private Material runtimeMaterial;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateDeckVisual(int cardsRemaining, int startingSize)
    {
        float percentageRemaining = (float)cardsRemaining / startingSize * 100f;

        deckCountText.text = cardsRemaining.ToString();

        if (runtimeMaterial == null)
        {
            runtimeMaterial = new Material(deckCountText.fontSharedMaterial);
            deckCountText.fontMaterial = runtimeMaterial;
        }
        runtimeMaterial.EnableKeyword("OUTLINE_ON");
        runtimeMaterial.SetFloat("_OutlineWidth", 0.2f);
        runtimeMaterial.SetColor("_OutlineColor", percentageRemaining >= 25f ? Color.green : Color.red);

        int total = deckStackImages.Count;
        for (int i = 0; i < total; i++)
        {
            float threshold = shrinkThreshhold(i, total, percentageRemaining);
            deckStackImages[i].SetActive(percentageRemaining > threshold);
        }
    }

    private float shrinkThreshhold(int i, int total, float percentageRemaining)
    {
        return 100f - ((i + 1) * (100f / total));
    }
}
