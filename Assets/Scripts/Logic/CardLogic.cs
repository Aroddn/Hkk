using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CardLogic: IIdentifiable, IComparable<CardLogic>
{
    public Player owner;
    public int UniqueCardID; 

    public CardAsset ca;
    public GameObject VisualRepresentation;
    public SpellEffect effect;

    //static dictionary to manage IDs
    public static Dictionary<int, CardLogic> CardsCreatedThisGame = new Dictionary<int, CardLogic>();

    public int ID
    {
        get{ return UniqueCardID; }
    }

    public int CurrentManaCost{ get; set; }

    public bool CanBePlayed
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.WhoseTurn == owner);
            bool fieldNotFull = true;
            if (ca.MaxHealth > 0)
                fieldNotFull = (owner.table.CreaturesOnTable.Count < 7);
            return ownersTurn && fieldNotFull && (CurrentManaCost <= owner.CurrentMana);
        }
    }

    public CardLogic(CardAsset ca)
    {
        this.ca = ca;
        UniqueCardID = IDFactory.GetUniqueID();
        ResetManaCost();
        if (ca.SpellScriptName != null && ca.SpellScriptName != "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.SpellScriptName)) as SpellEffect;
        }
        CardsCreatedThisGame.Add(UniqueCardID, this);
    }

    public int CompareTo(CardLogic other)
    {
        if (other.ca < this.ca)
        {
            return 1;
        }
        else if (other.ca > this.ca)
        {
            return -1;

        }
        else
            return 0;
    }

    public void ResetManaCost()
    {
        CurrentManaCost = ca.ManaCost;
    }
}
