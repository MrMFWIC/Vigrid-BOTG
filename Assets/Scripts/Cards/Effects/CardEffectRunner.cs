using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectRunner : MonoBehaviour
{
    public void RunEffect(List<CardEffectBase> effects, CardSO.EffectTriggerType triggerType, CardContext context)
    {
        if (effects == null || context == null)
        {
            Debug.LogWarning("Effects or context is null. Cannot run effects.");
            return;
        }

        foreach (var effect in effects)
        {
            if (effect.CanTrigger(context))
            {
                effect.ApplyEffect(context);
            }
        }
    }

    /* Example on how to use this in a game script
    CardContext context = new CardContext(playerGO, enemyGO, cardSO, cardSO.effectType);
    effectRunner.RunEffect(cardSO.onPlayEffects, context);
    */
}
