using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CreatureLogic: ICharacter
{
    // PUBLIC FIELDS
    public Player owner;
    public CardAsset ca;
    public CreatureEffect effect;
    public int UniqueCreatureID;
    public int ID
    {
        get { return UniqueCreatureID; }
    }
    public bool Frozen = false;

    //current
    private int health;

    //max
    private int baseHealth;

    public int MaxHealth
    {
        get{ return baseHealth;}
        set{ baseHealth = value;}
    }


    public int Health
    {
        get{ return health; }
        set{
            if (value <= 0)
                Die(false);
            else
                health = value;
        }
    }

    public void ChangeHealth(int value, bool heal)
    {
        GameObject creature = IDHolder.GetGameObjectWithID(ID);
        if (heal)
        {
            if (Health+value > MaxHealth)
            {
                Health = baseHealth;
                
                if (creature != null)
                    creature.GetComponent<OneCreatureManager>().HealthText.text = Health.ToString();
            }
            else
            {
                if (creature != null)
                    creature.GetComponent<OneCreatureManager>().Heal(value,Health+value);
            }
        }
        else
        {
            if (Health+value <=0 )
            {
                Die(false);
            }
            else
            {
                Health += value;
                MaxHealth = Health;

                if (creature != null)
                    creature.GetComponent<OneCreatureManager>().Buff(Attack, Health);
            }             
        }

    }

    public void FullHeal()
    {
        GameObject creature = IDHolder.GetGameObjectWithID(ID);
        if (!(Health > ca.MaxHealth))
        {
            Health = ca.MaxHealth;
        }

        if (creature != null)
            creature.GetComponent<OneCreatureManager>().Buff(Attack, Health);
    }

    private int attack;
    public int Attack
    {
        get { return attack; }
        set
        {
            GameObject creature = IDHolder.GetGameObjectWithID(ID);
            
            if (value < 0)
            {
                if(creature != null) creature.GetComponent<OneCreatureManager>().AttackText.text = 0.ToString();
                attack = 0;
            }
            else
            {
                attack = value;
                if (creature != null) creature.GetComponent<OneCreatureManager>().AttackText.text = value.ToString();
            }

        }
    }
   
    

    public bool CanAttack
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.WhoseTurn == owner);
            return (ownersTurn && (AttacksLeftThisTurn > 0) && !Frozen);
        }
    }
        
    private int attacksForOneTurn = 1;
    public int AttacksLeftThisTurn
    {
        get;
        set;
    }

    // CONSTRUCTOR
    public CreatureLogic(Player owner, CardAsset ca)
    {
        this.ca = ca;
        baseHealth = ca.MaxHealth;
        Health = ca.MaxHealth;
        Attack = ca.Attack;
        attacksForOneTurn = ca.AttacksForOneTurn;
        if (ca.Flying)
            AttacksLeftThisTurn = attacksForOneTurn;
        this.owner = owner;
        UniqueCreatureID = IDFactory.GetUniqueID();
        if (ca.CreatureScriptName!= null && ca.CreatureScriptName!= "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureScriptName), new System.Object[]{owner, this, ca.specialCreatureAmount}) as CreatureEffect;
            effect.RegisterEffect();
        }
        CreaturesCreatedThisGame.Add(UniqueCreatureID, this);
    }

    public void OnTurnStart()
    {
        AttacksLeftThisTurn = attacksForOneTurn;
    }

    public void Die(bool sacrifice)
    {
        
        new CreatureDieCommand(UniqueCreatureID, owner, sacrifice).AddToQueue();
    }

    public void GoFace()
    {
        AttacksLeftThisTurn--;
        int targetHealthAfter = owner.otherPlayer.Health - Attack;
        new CreatureAttackCommand(owner.otherPlayer.PlayerID, UniqueCreatureID, 0, Attack, Health, targetHealthAfter).AddToQueue();
        owner.otherPlayer.Health -= Attack;
    }

    public void AttackCreature (CreatureLogic target)
    {
        AttacksLeftThisTurn--;
        int targetHealthAfter = target.Health - Attack;
        int attackerHealthAfter = Health - target.Attack;
        new CreatureAttackCommand(target.UniqueCreatureID, UniqueCreatureID, target.Attack, Attack, attackerHealthAfter, targetHealthAfter).AddToQueue();

        target.Health -= Attack;
        Health -= target.Attack;
        //target.ChangeHealth(-Attack,false);
        //ChangeHealth(-target.Attack,false);
    }

    public void AttackCreatureWithID(int uniqueCreatureID)
    {
        CreatureLogic target = CreatureLogic.CreaturesCreatedThisGame[uniqueCreatureID];
        AttackCreature(target);
    }

    // STATIC For managing IDs
    public static Dictionary<int, CreatureLogic> CreaturesCreatedThisGame = new Dictionary<int, CreatureLogic>();

}
