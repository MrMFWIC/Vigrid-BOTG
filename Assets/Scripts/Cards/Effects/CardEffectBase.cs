using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffectBase : ScriptableObject, ICardEffect
{
    public virtual bool CanTrigger(CardContext context)
    {
        // Default implementation can be overridden in derived classes
        return true;
    }
    public abstract void ApplyEffect(CardContext context);
}
