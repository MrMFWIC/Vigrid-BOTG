using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardEffect
{
    bool CanTrigger(CardContext context);
    void ApplyEffect(CardContext context);
}
