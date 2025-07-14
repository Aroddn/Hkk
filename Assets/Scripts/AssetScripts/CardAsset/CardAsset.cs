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
    public ColorType secondColor = ColorType.NONE;
    public Set setName;
    public int releaseYear;
    public bool mental;
    public bool defense;
    public bool fire;
    public bool acid;
    public bool lightning;
    public bool anchor;
    public bool ancientMagic;
    public bool protection;
    public int reactionIncrease;
    public bool undead;
    //public int protectionLevel;

    [TextArea(2, 3)]
    public string Description;
    [TextArea(2, 3)]
    public string Tags;
    [TextArea(2, 3)]
    public string FlavorText;
    public Sprite CardImage;
    public int ManaCost;
    //todo figure out a way to represent other costs

    //public int ZanShardCost;
    //public int NekronCost;
    //public int BoneCost;
    //public int BuilderCost;

    public bool TokenCard = false;
    public int OverrideLimitOfThisCardInDeck = -1;

    public TypesOfCards TypeOfCard;

    [Header("Creature Info")]
    public int Attack;
    public bool magicAttack;
    public int MaxHealth;
    public bool magicDefense;
    public int Bone;
    public bool Flying;
    public bool Hand;
    public int AttacksForOneTurn = 1;
    public string CreatureScriptName;
    public int specialCreatureAmount;
    public MainType type;
    public SubType subType;
    public SubType secondSubType;


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
    Follower,
    Object,
    Building,
    Ship
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
    ALL,
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
    NONE,
    BUFA,
    COLORLESS
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
    ADVENTURER,
    AVATAR
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
    VISION,
    DRAKOLDER,
    XENÓ,
    NIGHTSPAWN,
    SKELETON,
    SIREN,
    GOLEM,
    SHAPE_CHANGER,
    SHADOWGNOME,
    BURÁSTYA,
    DINO,
    GANÜID,
    PIRATE,
    SPIDER,
    GIANT,
    QUWARG,
    TERMIK,
    THARGODAN,
    VAMPIRE,
    WOMATH,
    YETI,
    ELEMENTAL,
    GOBLIN,
    MINOTAUR,
    MOTYOGÓ,
    ANGEL,
    OCTOPUS,
    HUMAN
}

public enum Special
{
    FIRE,
    LIGHTNING,
    HAND,
    MONSTER_COMPONENT,
    ANCHOR,
    MENTAL,
    ACID,
    ICE
}

public enum Set
{
    All,
    AlfaAngels,
    AlfaBeasts,
    AlfaFireMagic,
    Gigászok,
    Moa_civilizáció
}
