using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContext
{
    public GameObject Owner { get; set; }
    public GameObject Target { get; set; }
    public CardSO CardSource { get; set; } 
    public CardSO.EffectTriggerType TriggerType { get; set; }

    public CardContext(GameObject owner, GameObject target, CardSO cardSource, CardSO.EffectTriggerType triggerType)
    {
        Owner = owner;
        Target = target;
        CardSource = cardSource;
        TriggerType = triggerType;
    }
}
