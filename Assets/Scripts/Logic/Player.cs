using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using DG.Tweening.Core.Easing;
using System;
using System.Reflection;

//MOST IMPORTANT AND COMPLICATED SCRIPT

public enum PlayerType { PLAYER, ENEMY };

public class Player : NetworkBehaviour, ICharacter
{
    [Header("Player Info")]
    [SyncVar(hook = nameof(UpdatePlayerName))] public string username;

    public int PlayerID;
    public CharacterAsset charAsset;
    public PlayerArea PArea;

    [SyncVar]
    public string deckName;

    [SyncVar]
    public string charAssetName;

    [SyncVar]
    public List<string> cardNames = new List<string>();


    public Deck deck;
    public Hand hand;
    public Table table;
    public GraveYard graveYard;
    public Void voiid;

    private int TurnCounter;


     public static Player localPlayer;
     public bool hasEnemy = false;
     public PlayerInfo enemyInfo;

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
        deckName = newDeckName;
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isClientOnly)
        {

            foreach (string name in cardNames)
            {
                {
                    this.deck.cards.Add(CardCollection.Instance.GetCardAssetByName(name));
                }
            }
            this.charAsset = CardCollection.Instance.GetCharacterAssetByName(charAssetName);

        }
    }

    [Command]
    public void CmdLoadPlayer(string user)
    {
        username = user;
    }
    void UpdatePlayerName(string oldUser, string newUser)
    {
        username = newUser;
        gameObject.name = newUser;
    }


    public void Update()
    {
        if (!hasEnemy && username != "")
        {
            UpdateEnemyInfo();
        }
    }

    public void UpdateEnemyInfo()
    {
        Player[] onlinePlayers = FindObjectsOfType<Player>();

        foreach (Player player in onlinePlayers)
        {
            if (player.username != "")
            {
                if (player != this)
                {
                    PlayerInfo currentPlayer = new PlayerInfo(player.gameObject);
                    enemyInfo = currentPlayer;
                    hasEnemy = true;
                    GlobalSettings.Instance.LowPlayer = localPlayer;
                    GlobalSettings.Instance.TopPlayer = localPlayer.otherPlayer;
                    GlobalSettings.Instance.Players.Clear();
                    GlobalSettings.Instance.Players.Add(AreaPosition.Top, GlobalSettings.Instance.TopPlayer);
                    GlobalSettings.Instance.Players.Add(AreaPosition.Low, GlobalSettings.Instance.LowPlayer);
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
            new UpdateManaCommand(this, maxMana, currentMana).AddToQueue();
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
            new UpdateManaCommand(this, MaxMana, currentMana).AddToQueue();


            if (TurnManager.Instance.WhoseTurn == this) PlayableCardHighlighter();
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
        new PlayASpellCardCommand(this, playedCard).AddToQueue();
        hand.CardsInHand.Remove(playedCard);

        if (PArea.owner == AreaPosition.Low)
        {
            TurnManager.playerAction.Add(PlayerAction.PlayerLowAction);
        }
        else
        {
            TurnManager.playerAction.Add(PlayerAction.PlayerTopAction);
        }

        TurnManager.Instance.GiveControlToOtherPlayer();
    }

    [Command]
    public void CmdPlayCreature(int UniqueID, int tablePos)
    {
        GameObject creature = Instantiate(GlobalSettings.Instance.CreaturePrefab);
        NetworkServer.Spawn(creature);
        RpcPlayCreature(UniqueID, tablePos,creature);
    }

    [ClientRpc]
    void RpcPlayCreature(int UniqueID, int tablePos, GameObject creature)
    {
        var card = CardLogic.CardsCreatedThisGame[UniqueID];
        PlayACreatureFromHand(card, tablePos,creature);
    }

    public void PlayACreatureFromHand(CardLogic playedCard, int tablePos, GameObject creature)
    {
        CurrentMana -= playedCard.CurrentManaCost;
        CreatureLogic newCreature = new CreatureLogic(this, playedCard.ca);
        table.CreaturesOnTable.Insert(tablePos, newCreature);
        new PlayACreatureCommand(playedCard, this, tablePos, newCreature,creature).AddToQueue();
        hand.CardsInHand.Remove(playedCard);

        if (PArea.owner == AreaPosition.Low)
            TurnManager.playerAction.Add(PlayerAction.PlayerLowAction);
        else
            TurnManager.playerAction.Add(PlayerAction.PlayerTopAction);

        if (newCreature.effect != null)
            newCreature.effect.WhenACreatureIsPlayed();

        TurnManager.Instance.GiveControlToOtherPlayer();
    }


    public void Die()
    {
        PArea.ControlsON = false;
        otherPlayer.PArea.ControlsON = false;
        TurnManager.Instance.StopTheTimer();
        new GameOverCommand(this).AddToQueue();
    }

    public void PlayableCardHighlighter(bool removeAllHighlights = false)
    {
        foreach (CardLogic cl in hand.CardsInHand){
            GameObject g = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
            if (g!=null)
                g.GetComponent<OneCardManager>().CanBePlayedNow = (cl.CurrentManaCost <= CurrentMana) && !removeAllHighlights;
        }

        foreach (CreatureLogic crl in table.CreaturesOnTable){
            GameObject g = IDHolder.GetGameObjectWithID(crl.UniqueCreatureID);
            if(g!= null)
                g.GetComponent<OneCreatureManager>().CanAttackNow = (crl.AttacksLeftThisTurn > 0) && !removeAllHighlights;
        }          
    }

    public void LoadCharacterInfoFromAsset(){
        Health = charAsset.MaxHealth;
        MaxHealth = charAsset.MaxHealth;
        PArea.Portrait.charAsset = charAsset;
        PArea.Portrait.ApplyLookFromAsset();
    }

    public void TransmitInfoAboutPlayerToVisual(){
        PArea.Portrait.gameObject.AddComponent<IDHolder>().UniqueID = PlayerID;
        PArea.AllowedToControlThisPlayer = true;
    }

    //Not implemented yet
    public void Die(bool sacrifice){throw new System.NotImplementedException();}
    public void GetCardOutsideOfGame(CardAsset cardAsset) { }

}
