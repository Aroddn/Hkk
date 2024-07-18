using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class CardAsset : ScriptableObject 
{
    [Header("General info")]
    //public CharacterAsset characterAsset;
    public Rarity rarity;
    public Color color;
    public int releaseYear;

    //public bool reaction
    [TextArea(2,3)]
    public string Description;
	public Sprite CardImage;
    public int ManaCost;



    [Header("Creature Info")]
    public int MaxHealth;
    public int Attack;
    public int AttacksForOneTurn = 1;
    public bool Taunt;
    public bool Flying;
    public string CreatureScriptName;
    public int specialCreatureAmount;
    public Type type;
    public SubType subType;

    

    [Header("SpellInfo")]
    public string SpellScriptName;
    public int specialSpellAmount;
    public PlaySpeed playSpeed;
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
    PISHOGUE,
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
    MUTANT
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
