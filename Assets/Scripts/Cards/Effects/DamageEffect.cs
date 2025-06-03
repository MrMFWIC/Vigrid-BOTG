using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effects/Damage Effect")]
public class DamageEffect : CardEffectBase
{
    public int damageAmount;

    public override void ApplyEffect(GameObject owner, GameObject target)
    {
        var health = target.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
            Debug.Log($"{owner.name} deals {damageAmount} damage to {target.name}");
        }
        else
        {
            Debug.LogWarning($"Target {target.name} does not have a HealthComponent.");
        }
    }
}
