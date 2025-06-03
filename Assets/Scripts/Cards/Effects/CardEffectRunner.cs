using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectRunner : MonoBehaviour
{
    public void RunEffect(List<CardEffectBase> effects, GameObject owner, GameObject target)
    {
        if (effects == null) return;

        foreach (var effect in effects)
        {
            effect?.ApplyEffect(owner, target);
        }
    }
}
