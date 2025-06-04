using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effects/Damage Effect")]
public class DamageEffect : CardEffectBase
{
    public int damageAmount;

    public override void ApplyEffect(CardContext context)
    {
        var health = context.Target.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
            Debug.Log($"{context.Owner.name} deals {damageAmount} damage to {context.Target.name}");
        }
        else
        {
            Debug.LogWarning($"Target {context.Target.name} does not have a HealthComponent.");
        }
    }
}
