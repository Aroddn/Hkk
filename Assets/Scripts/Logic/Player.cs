using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ICharacter
{
    // PUBLIC FIELDS
    // int ID that we get from ID factory
    public int PlayerID;
    public CharacterAsset charAsset;
    public PlayerArea PArea;
    public SpellEffect HeroPowerEffect;


    // REFERENCES TO LOGICAL STUFF THAT BELONGS TO THIS PLAYER
    public Deck deck;
    public Hand hand;
    public Table table;

    private int TurnCounter;
    private int bonusManaThisTurn = 0;
    //public bool usedHeroPowerThisTurn = false;


    public int ID
    {
        get { return PlayerID; }
    }

    private int manaThisTurn;//maxmana
    public int ManaThisTurn
    {
        get{ return manaThisTurn;}
        set
        {
            manaThisTurn = value;
            //PArea.ManaBar.CurrentMana = manaThisTurn;
            new UpdateManaCrystalsCommand(this, manaThisTurn, manaLeft).AddToQueue();
        }
    }

    private int totalBones;


    public int TotalBones
    {
        get { return totalBones; }
        set
        {
            totalBones = value;
            new UpdateBonesCommand(this, totalBones).AddToQueue();
        }
    }

    private int manaLeft;//currentMana

    public int ManaLeft
    {
        get
        { return manaLeft;}
        set
        {
            manaLeft = value;
            //PArea.ManaBar.CurrentMana = manaLeft;
            new UpdateManaCrystalsCommand(this, ManaThisTurn, manaLeft).AddToQueue();
            //Debug.Log(ManaLeft);
            if (TurnManager.Instance.whoseTurn == this)
                HighlightPlayableCards();
        }
    }

    public Player otherPlayer
    {
        get
        {
            if (Players[0] == this)
                return Players[1];
            else
                return Players[0];
        }
    }

    private int health;
    public int Health
    {
        get { return health;}
        set
        {
            health = value;
            if (value <= 0)
                Die(); 
        }
    }

    public delegate void VoidWithNoArguments();
    //public event VoidWithNoArguments CreaturePlayedEvent;
    //public event VoidWithNoArguments SpellPlayedEvent;
    //public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;

    public static Player[] Players;

    void Awake()
    {
        Players = GameObject.FindObjectsOfType<Player>();
        PlayerID = IDFactory.GetUniqueID();
    }

    public virtual void OnTurnStart()
    {
        TurnCounter++;
        ManaLeft += TurnCounter;

        foreach (CreatureLogic cl in table.CreaturesOnTable)
            cl.OnTurnStart();
    }

    public void GetBonusMana(int amount)
    {
        bonusManaThisTurn += amount;
        ManaThisTurn += amount;
        ManaLeft += amount;
    }   

    public void OnTurnEnd()
    {
        if(EndTurnEvent != null)
            EndTurnEvent.Invoke();
        ManaThisTurn -= bonusManaThisTurn;
        bonusManaThisTurn = 0;
        GetComponent<TurnMaker>().StopAllCoroutines();
    }

    public void DrawACard(bool fast = false)//false
    {
        if (deck.cards.Count > 0)
        {
            if (hand.CardsInHand.Count < PArea.handVisual.slots.Children.Length)
            {
                // 1) logic: add card to hand
                CardLogic newCard = new CardLogic(deck.cards[0]);
                newCard.owner = this;
                hand.CardsInHand.Insert(0, newCard);
                // Debug.Log(hand.CardsInHand.Count);
                // 2) logic: remove the card from the deck
                deck.cards.RemoveAt(0);
                // 2) create a command
                new DrawACardCommand(hand.CardsInHand[0], this, fast, fromDeck: true).AddToQueue();
            }
        }
        else
        {
            //nothing happens cause in this game you cant deck out
        }
       
    }


    public void GetACardNotFromDeck(CardAsset cardAsset)
    {
        if (hand.CardsInHand.Count < PArea.handVisual.slots.Children.Length)
        {
            // 1) logic: add card to hand
            CardLogic newCard = new CardLogic(cardAsset);
            newCard.owner = this;
            hand.CardsInHand.Insert(0, newCard);
            // 2) send message to the visual Deck
            new DrawACardCommand(hand.CardsInHand[0], this, fast: true, fromDeck: false).AddToQueue();
        }
        // no removal from deck because the card was not in the deck
    }

    public void PlayASpellFromHand(int SpellCardUniqueID, int TargetUniqueID)
    {
        // TODO: !!!
        // if TargetUnique ID < 0 , for example = -1, there is no target.
        if (TargetUniqueID < 0)
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], null);
        else if (TargetUniqueID == ID)
        {
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], this);
        }
        else if (TargetUniqueID == otherPlayer.ID)
        {
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], this.otherPlayer);
        }
        else
        {
            // target is a creature
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], CreatureLogic.CreaturesCreatedThisGame[TargetUniqueID]);
        }
          
    }

    public void PlayASpellFromHand(CardLogic playedCard, ICharacter target)
    {
        ManaLeft -= playedCard.CurrentManaCost;
        // cause effect instantly:
        if (playedCard.effect != null)
            playedCard.effect.ActivateEffect(playedCard.ca.specialSpellAmount, target);
        else
        {
            Debug.LogWarning("No effect found on card " + playedCard.ca.name);
        }
        // no matter what happens, move this card to PlayACardSpot
        new PlayASpellCardCommand(this, playedCard).AddToQueue();
        // remove this card from hand
        hand.CardsInHand.Remove(playedCard);
        // check if this is a creature or a spell
    }

    public void PlayACreatureFromHand(int UniqueID, int tablePos)
    {
        PlayACreatureFromHand(CardLogic.CardsCreatedThisGame[UniqueID], tablePos);

    }

    public void PlayACreatureFromHand(CardLogic playedCard, int tablePos)
    {
        ManaLeft -= playedCard.CurrentManaCost;
        // create a new creature object and add it to Table
        CreatureLogic newCreature = new CreatureLogic(this, playedCard.ca);
        table.CreaturesOnTable.Insert(tablePos, newCreature);
        // no matter what happens, move this card to PlayACardSpot
        new PlayACreatureCommand(playedCard, this, tablePos, newCreature.UniqueCreatureID).AddToQueue();
        // remove this card from hand
        hand.CardsInHand.Remove(playedCard);
        HighlightPlayableCards();
    }


    public void Die()
    {
        // game over
        // block both players from taking new moves 
        PArea.ControlsON = false;
        otherPlayer.PArea.ControlsON = false;
        TurnManager.Instance.StopTheTimer();
        new GameOverCommand(this).AddToQueue();
    }

    // METHODS TO SHOW GLOW HIGHLIGHTS
    public void HighlightPlayableCards(bool removeAllHighlights = false)
    {
        //TODO if enemies turn highlight those that can be used with your acition but on enemies turn

        foreach (CardLogic cl in hand.CardsInHand)
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
            if (g!=null)
                g.GetComponent<OneCardManager>().CanBePlayedNow = (cl.CurrentManaCost <= ManaLeft) && !removeAllHighlights;
        }

        foreach (CreatureLogic crl in table.CreaturesOnTable)
        {
            GameObject g = IDHolder.GetGameObjectWithID(crl.UniqueCreatureID);
            if(g!= null)
                g.GetComponent<OneCreatureManager>().CanAttackNow = (crl.AttacksLeftThisTurn > 0) && !removeAllHighlights;
        }
            

    }

    // START GAME METHODS
    public void LoadCharacterInfoFromAsset()
    {
        Health = charAsset.MaxHealth;
        PArea.Portrait.charAsset = charAsset;
        PArea.Portrait.ApplyLookFromAsset();
    }

    public void TransmitInfoAboutPlayerToVisual()
    {
        PArea.Portrait.gameObject.AddComponent<IDHolder>().UniqueID = PlayerID;
        if (GetComponent<TurnMaker>() is AITurnMaker)
        {
            // turn off turn making for this character
            PArea.AllowedToControlThisPlayer = false;
        }
        else
        {
            // allow turn making for this character
            PArea.AllowedToControlThisPlayer = true;
        }
    }


    //TODO what is this?
    public void Die(bool sacrifice)
    {
        throw new System.NotImplementedException();
    }
}
