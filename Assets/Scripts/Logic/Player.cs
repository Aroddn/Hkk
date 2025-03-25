using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using DG.Tweening.Core.Easing;
using Mirror.Examples.MultipleMatch;

public enum PlayerType { PLAYER, ENEMY };

public class Player : NetworkBehaviour, ICharacter
{
    [Header("Player Info")]
    [SyncVar(hook = nameof(UpdatePlayerName))] public string username;

    // PUBLIC FIELDS
    public int PlayerID;
    public CharacterAsset charAsset;
    public PlayerArea PArea;

    [SyncVar]
    public string deckName;

    [SyncVar]
    public List<string> cardNames = new List<string>();


    public Deck deck;
    public Hand hand;
    public Table table;
    public GraveYard graveYard;
    public Void voiid;

    private int TurnCounter;


     public static Player localPlayer;
     public bool hasEnemy = false; // If we have set an enemy.
     public PlayerInfo enemyInfo; // We can't pass a Player class through the Network, but we can pass structs. 
    // We store all our enemy's info in a PlayerInfo struct so we can pass it through the network when needed.
    public static GameManager gameManager;
    [SyncVar] public bool firstPlayer = false;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        localPlayer = this;
        CmdLoadPlayer(PlayerPrefs.GetString("Name"));
    }

    [Command(requiresAuthority = false)]
    public void CmdSetPlayerData(string newDeckName, List<string> newCardNames)
    {
        // Update SyncVars (automatically syncs with clients)
        deckName = newDeckName;
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isClientOnly)
        {
            Debug.Log($"Player initialized with deck: {deckName}, cards: {string.Join(", ", cardNames)}");
        }
    }

    [Command]
    public void CmdLoadPlayer(string user)
    {
        username = user;
    }
    void UpdatePlayerName(string oldUser, string newUser)
    {
        // Update username
        username = newUser;

        // Update game object's name in editor (only useful for debugging).
        gameObject.name = newUser;
    }


    public void Update()
    {
        // Get EnemyInfo as soon as another player connects. Only start updating once our Player has been loaded in properly (username will be set if loaded in).
        if (!hasEnemy && username != "")
        {
            UpdateEnemyInfo();
        }
    }

    public void UpdateEnemyInfo()
    {
        // Find all Players and add them to the list.
        Player[] onlinePlayers = FindObjectsOfType<Player>();

        // Loop through all online Players (should just be one other Player)
        foreach (Player player in onlinePlayers)
        {

            // Make sure the players are loaded properly (we load the usernames first)
            if (player.username != "")
            {
                // There should only be one other Player online, so if it's not us then it's the enemy.
                if (player != this)
                {
                    // Get & Set PlayerInfo from our Enemy's gameObject
                    PlayerInfo currentPlayer = new PlayerInfo(player.gameObject);
                    enemyInfo = currentPlayer;
                    hasEnemy = true;
                    //enemyInfo.data.casterType = Target.OPPONENT;
                    GlobalSettings.Instance.LowPlayer = localPlayer;
                    GlobalSettings.Instance.TopPlayer = localPlayer.otherPlayer;
                    GlobalSettings.Instance.Players.Add(AreaPosition.Top, GlobalSettings.Instance.TopPlayer);
                    GlobalSettings.Instance.Players.Add(AreaPosition.Low, GlobalSettings.Instance.TopPlayer);
                    localPlayer.PArea = GameObject.Find("LowerPlayerArea").GetComponent<PlayerArea>();
                    localPlayer.otherPlayer.PArea = GameObject.Find("TopPlayerArea").GetComponent<PlayerArea>();
                }
            }
        }


    }

    public int ID
    {
        get { return PlayerID; }
    }

    private int maxMana;
    public int MaxMana
    {
        get{ return maxMana; }
        set
        {
            maxMana = value;
            new UpdateManaCrystalsCommand(this, maxMana, currentMana).AddToQueue();
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

    private int currentMana;

    public int CurrentMana
    {
        get
        { return currentMana; }
        set
        {
            currentMana = value;
            new UpdateManaCrystalsCommand(this, MaxMana, currentMana).AddToQueue();
            if (TurnManager.Instance.WhoseTurn == this)
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

    private int maxHealth;

    public int MaxHealth {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    private int health;
    public int Health
    {
        get { return health;}
        set { health = value;
            if (value > charAsset.MaxHealth)
            {
                health = charAsset.MaxHealth;
            }
            else
            {
                health = value;
            }
            if (value <= 0)
                Die(); 
        }
    }

    public void ChangeHealth(int value, bool heal)
    {
        GameObject creature = IDHolder.GetGameObjectWithID(ID);
        if (heal)
        {
            if (Health + value > MaxHealth)
            {
                Health = MaxHealth;
            } else if (Health + value < 0)
                Die();
            else
            {
                Health += value;
            }
        }
        else
        {
            if (MaxHealth + value < 0)
            {
                Die();
            }
            else
            {
                MaxHealth += value;
            }
        }
    }

    int ICharacter.Attack { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public delegate void VoidWithNoArguments();
    //public event VoidWithNoArguments CreaturePlayedEvent;
    //public event VoidWithNoArguments SpellPlayedEvent;
    public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;

    public static Player[] Players;

    void Awake()
    {
        Players = GameObject.FindObjectsOfType<Player>();
        PlayerID = IDFactory.GetUniqueID();
        //PArea = GameObject.Find("LowerPlayerArea").GetComponent<PlayerArea>();
    }

    public virtual void OnTurnStart()
    {
        TurnCounter++;
        CurrentMana += TurnCounter;

        if (StartTurnEvent != null)
            StartTurnEvent.Invoke();
        GetComponent<TurnMaker>().StopAllCoroutines();

        //enemy creatures
        foreach (CreatureLogic cl in this.otherPlayer.table.CreaturesOnTable)
        {
            cl.FullHeal();
            cl.Attack += 1;
            cl.ChangeHealth(1, false);   
        }

        //your creatures
        foreach (CreatureLogic cl in table.CreaturesOnTable)
        {
            cl.OnTurnStart();           
            cl.Attack -= 1;
            if (cl.Health-1 >0)
            {
                cl.ChangeHealth(-1, false);
            }
            cl.FullHeal();
        }    
            
    }

    public void GetBonusMana(int amount)
    {
        CurrentMana += amount;
    }   

    public void OnTurnEnd()
    {
        if(EndTurnEvent != null)
            EndTurnEvent.Invoke();
        GetComponent<TurnMaker>().StopAllCoroutines();
    }

    public void DrawACard(bool fast = false)
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
        CurrentMana -= playedCard.CurrentManaCost;
        if (playedCard.effect != null)
            playedCard.effect.ActivateEffect(playedCard.ca.specialSpellAmount, playedCard.ca.specialSpellAmount2, target);
        else
        {
            Debug.LogWarning("No effect found on card " + playedCard.ca.name);
        }
        // no matter what happens, move this card to PlayACardSpot
        new PlayASpellCardCommand(this, playedCard).AddToQueue();
        // remove this card from hand
        hand.CardsInHand.Remove(playedCard);

        if (PArea.owner == AreaPosition.Low)//ID == 2
        {
            TurnManager.playerAction.Add(PlayerAction.PlayerLowAction);
        }
        else
        {
            TurnManager.playerAction.Add(PlayerAction.PlayerTopAction);
        }

        //Debug.Log("A spell has been played");

        TurnManager.Instance.GiveControlToOtherPlayer();
    }

    public void PlayACreatureFromHand(int UniqueID, int tablePos)
    {
        PlayACreatureFromHand(CardLogic.CardsCreatedThisGame[UniqueID], tablePos);

    }

    public void PlayACreatureFromHand(CardLogic playedCard, int tablePos)
    {
        CurrentMana -= playedCard.CurrentManaCost;
        // create a new creature object and add it to Table
        CreatureLogic newCreature = new CreatureLogic(this, playedCard.ca);
        table.CreaturesOnTable.Insert(tablePos, newCreature);
        // no matter what happens, move this card to PlayACardSpot
        new PlayACreatureCommand(playedCard, this, tablePos, newCreature.UniqueCreatureID).AddToQueue();
        // remove this card from hand
        hand.CardsInHand.Remove(playedCard);

        if (PArea.owner == AreaPosition.Low)
        {
            TurnManager.playerAction.Add(PlayerAction.PlayerLowAction);
        }
        else
        {
            TurnManager.playerAction.Add(PlayerAction.PlayerTopAction);
        }

        if (newCreature.effect != null)
            newCreature.effect.WhenACreatureIsPlayed();

        TurnManager.Instance.GiveControlToOtherPlayer();
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
                g.GetComponent<OneCardManager>().CanBePlayedNow = (cl.CurrentManaCost <= CurrentMana) && !removeAllHighlights;
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
        MaxHealth = charAsset.MaxHealth;
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

    public void Die(bool sacrifice)
    {
        throw new System.NotImplementedException();
    }
}
