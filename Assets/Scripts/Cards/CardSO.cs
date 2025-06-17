using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class CardSO : ScriptableObject
{
    public string cardID; // Unique identifier for the card, can be used for serialization or database lookups

    [Header("Info")]
    public CardType cardType;
    public AttackLimitations attackLimitations;
    public EffectTriggerType effectType;

    [Header("Text")]
    public string cardName;
    public string cardEffect;
    public int cardCost;
    public int cardAttack;
    public string cardLore;

    [Header("Visuals")]
    public Sprite cardImage;
    public Affiliation affiliation;

    [Header("Effects")]
    public List<CardEffectBase> onPlayEffects;
    public List<CardEffectBase> onAttackEffects;
    public List<CardEffectBase> onDefendEffects;
    public List<CardEffectBase> onDeathEffects;
    public List<CardEffectBase> fromHandEffects;

    public enum CardType
    {
        Unit,
        Spell
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(cardID))
        {
            cardID = $"{Regex.Replace(cardName.ToLower(), "[^a-z0-9]", "")}_001";
        }
    }
#endif

    public enum Affiliation
    {
        Human,
        Dark,
        Mystic,
        Beast,
        Spell
    }

    public enum AttackLimitations
    {
        UnitOnly,
        DirectOnly,
        UnitOrDirect,
        Standard,
        CantAttack
    }

    public enum EffectTriggerType
    {
        OnPlay,
        OnAttack,
        OnDefend,
        OnDeath,
        FromHand,
        FromDeck,
        Passive,
        Conditional,
        Optional,
        Spell
    }

    public Color GetAffiliationColor()
    {
        return affiliation switch
        {
            Affiliation.Human => Color.green,
            Affiliation.Dark => Color.grey,
            Affiliation.Mystic => Color.blue,
            Affiliation.Beast => Color.red,
            Affiliation.Spell => Color.white,
            _ => Color.black
        };
    }
}