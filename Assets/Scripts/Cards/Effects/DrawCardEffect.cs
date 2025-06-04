using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effects/Draw Card Effect")]
public class DrawCardEffect : CardEffectBase
{
    public int cardsToDraw;

    public override void ApplyEffect(CardContext context)
    {
        DeckManager deckManager = context.Owner.GetComponentInParent<DeckManager>();
        HandManager handManager = context.Owner.GetComponentInParent<HandManager>();

        if (deckManager == null || handManager == null)
        {
            Debug.LogWarning("DeckManager or HandManager missing from context.Owner.");
            return;
        }

        for (int i = 0; i < cardsToDraw; i++)
        {
            deckManager.DrawCard(handManager);
        }

        Debug.Log($"{context.Owner.name} triggered DrawCardEffect and drew {cardsToDraw} card(s).");
    }
}
