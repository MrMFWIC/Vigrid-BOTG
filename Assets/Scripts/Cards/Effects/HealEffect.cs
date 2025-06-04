using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effects/Heal Effect")]
public class HealEffect : CardEffectBase
{
    public int healAmount;

    public override void ApplyEffect(CardContext context)
    {
        var health = context.Target.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.Heal(healAmount);
            Debug.Log($"{context.Owner.name} heals {healAmount} to {context.Target.name}");
        }
        else
        {
            Debug.LogWarning($"Target {context.Target.name} does not have a HealthComponent.");
        }
    }
}
