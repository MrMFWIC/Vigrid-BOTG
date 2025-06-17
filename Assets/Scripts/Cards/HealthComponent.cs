using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthComponent : MonoBehaviour
{
    public CardSO cardData; // Reference to the CardSO scriptable object
    public int startingHealth;
    public int currentHealth;

    private void Awake()
    {
        /*if (SceneManager.GetActiveScene().name != "Battlefield")
        {
            this.enabled = false; // Disable this script if not in the Battlefield scene
        }*/

        cardData = GetComponent<CardDisplay>().cardInstance;
        startingHealth  = cardData.cardAttack;
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        // Handle taking damage, trigger on-damage effects
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            //Update ATK value here
            UpdateATKValue();
        }
    }

    public void Heal(int amount)
    {
        // Handle healing, trigger on-heal effects
        currentHealth += amount;
        Debug.Log($"{gameObject.name} healed {amount}. Current health: {currentHealth}");

        // Update ATK value here
        UpdateATKValue();
    }

    void UpdateATKValue()
    {
        cardData.cardAttack = currentHealth;
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        if (cardDisplay != null)
        {
            cardDisplay.UpdateCardDisplay();
        }
        else
        {
            Debug.LogError("CardDisplay component not found on the game object.");
        }
    }

    void Die()
    {
        // Handle death logic, trigger death effects
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}

