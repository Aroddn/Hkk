using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class CardAsset : ScriptableObject 
{
    [Header("General info")]
    public Rarity rarity;
    public Color color;
    public int releaseYear;
    public bool mental;
    public bool defense;
    public bool fire;
    public bool anchor;

    //public bool reaction
    [TextArea(2, 3)]
    public string Description;
    [TextArea(2, 3)]
    public string FlavorText;
    public Sprite CardImage;
    public int ManaCost;

    [Header("Creature Info")]
    public int MaxHealth;
    public int Attack;
    public int AttacksForOneTurn = 1;
    public int Bone;
    public bool Flying;
    public bool Hand;
    public string CreatureScriptName;
    public int specialCreatureAmount;
    public Type type;
    public SubType subType;


    [Header("SpellInfo")]
    public string SpellScriptName;
    public int specialSpellAmount;
    public PlaySpeed playSpeed;
    public SubType spellSubType;
    public TargetingOptions Targets;
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

public enum Rarity
{
    COMMON,
    UNCOMMON,
    RARE,
    ULTRARARE
}

public enum Color
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


public enum Type
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
