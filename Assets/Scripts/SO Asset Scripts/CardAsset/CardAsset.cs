using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class CardAsset : ScriptableObject, IComparable<CardAsset>
{
    [Header("General info")]
    public string cardName;
    public RarityOptions Rarity;
    public ColorType color;
    public int releaseYear;
    public bool mental;
    public bool defense;
    public bool fire;
    public bool anchor;

    [TextArea(2, 3)]
    public string Description;
    [TextArea(2, 3)]
    public string Tags;
    [TextArea(2, 3)]
    public string FlavorText;
    public Sprite CardImage;
    public int ManaCost;
    public bool TokenCard = false;
    public int OverrideLimitOfThisCardInDeck = -1;

    public TypesOfCards TypeOfCard;

    [Header("Creature Info")]
    public int MaxHealth;
    public int Attack;
    public int AttacksForOneTurn = 1;
    public int Bone;
    public bool Flying;
    public bool Hand;
    public string CreatureScriptName;
    public int specialCreatureAmount;
    public MainType type;
    public SubType subType;


    [Header("SpellInfo")]
    public string SpellScriptName;
    public int specialSpellAmount;
    public int specialSpellAmount2;
    public PlaySpeed playSpeed;
    public SubType spellSubType;
    public TargetingOptions Targets;

    public int CompareTo(CardAsset other)
    {
        if (other.ManaCost < this.ManaCost)
        {
            return 1;
        }
        else if (other.ManaCost > this.ManaCost)
        {
            return -1;
        }
        else
        {
            // if mana costs are equal sort in alphabetical order
            return name.CompareTo(other.name);
        }
    }

    // Define the is greater than operator.
    public static bool operator >(CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) == 1;
    }

    // Define the is less than operator.
    public static bool operator <(CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) == -1;
    }

    // Define the is greater than or equal to operator.
    public static bool operator >=(CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }

    // Define the is less than or equal to operator.
    public static bool operator <=(CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }
}

public enum TypesOfCards
{
    Creature,
    Spell,
    Follower
}
public enum TargetingOptions
{
    NoTarget,
    AllCreatures,
    EnemyCreatures,
    YourCreatures,
    AllCharacters,
    EnemyCharacters,
    YourCharacters
}

public enum RarityOptions
{
    Basic,
    COMMON,
    UNCOMMON,
    RARE,
    ULTRARARE
}

public enum ColorType
{
    CHARADIN,
    DORNODON,
    FAIRLIGHT,
    LEAH,
    RAIA,
    RHATT,
    SHERAN,
    THARR,
    ELEINOS,
    NONE
}

public enum PlaySpeed
{
    INSTANT,
    GENERAL,
    GLAMOUR,
    REACTION
}


public enum MainType
{
    MONSTER,
    ADVENTURER
}

public enum SubType
{
    NONE,
    ORK,
    DRAGON,
    PEGASUS,
    PHOENIX,
    GNOME,
    DRAGON_PHOENIX,
    SNAKE,
    GRIFF,
    ELF,
    MUTANT,
    REACTION,
    VISION
}

public enum Special
{
    FIRE,
    LIGHTNING,
    HAND,
    MONSTER_COMPONENT,
    ANCHOR,
    MENTAL
}
