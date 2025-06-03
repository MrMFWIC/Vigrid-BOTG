using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effects/Damage Effect")]
public class HealEffect : CardEffectBase
{
    public int healAmount;

    public override void ApplyEffect(GameObject owner, GameObject target)
    {
        var health = target.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.Heal(healAmount);
            Debug.Log($"{owner.name} heals {healAmount} to {target.name}");
        }
        else
        {
            Debug.LogWarning($"Target {target.name} does not have a HealthComponent.");
        }
    }
}
