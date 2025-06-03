using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class CardSO : ScriptableObject
{
    [Header("Info")]
    public CardType cardType;
    public AttackLimitations attackLimitations;
    public EffectType effectType;

    [Header("Text")]
    public string cardName;
    public string cardEffect;
    public int cardCost;
    public int cardAttack;

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

    public enum EffectType
    {
        OnPlay,
        OnAttack,
        OnDefend,
        OnDeath,
        FromHand,
        FromDeck,
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