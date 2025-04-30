using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System;
using Unity.Collections.LowLevel.Unsafe;
using System.Linq;
using Mirror;

public class GlobalSettings: MonoBehaviour 
{
    [Header("Players")]
    public Player TopPlayer;
    public Player LowPlayer;
    [Header("Numbers and Values")]
    public float CardPreviewTime = 1f;
    public float CardTransitionTime= 1f;
    public float CardPreviewTimeFast = 0.2f;
    public float CardTransitionTimeFast = 0.5f;
    [Header("Prefabs and Assets")]
    public GameObject NoTargetSpellCardPrefab;
    public GameObject TargetedSpellCardPrefab;
    public GameObject CreatureCardPrefab;
    public GameObject CreaturePrefab;
    public GameObject DamageEffectPrefab;
    public GameObject ExplosionPrefab;
    [Header("Other")]
    public Button EndTurnButton;
    public GameObject GameOverCanvas;

    public Dictionary<AreaPosition, Player> Players = new Dictionary<AreaPosition, Player>();

    public static GlobalSettings Instance;

    void Awake()
    {
        Instance = this;
    }

    //not used anymore in main project to keep it for future development
    public void AssignDeckAndCharacter(Player player, DeckInfo deckInfo)
    {
        if (deckInfo != null)
        {
            if (deckInfo.Character != null)
                player.charAsset = deckInfo.Character;

            if (deckInfo.Cards != null)
                player.deck.cards = new List<CardAsset>(deckInfo.Cards);

            player.deck.cards.Shuffle();
        }
        else
        {
            player.deck.cards = LoadDefaultDeck(1);
        }
    }

    public List<CardAsset> LoadDefaultDeck(int num)
    {
        List<CardAsset> deck1 = new List<CardAsset>();
        List<CardAsset> deck2 = new List<CardAsset>();
        List<CardAsset> deck3 = new List<CardAsset>();

        deck1 = Resources.LoadAll<CardAsset>("MyAssests/CardAssets/Decks/Angels").ToList();
        deck2 = Resources.LoadAll<CardAsset>("MyAssests/CardAssets/Decks/FireMagic").ToList();
        deck3 = Resources.LoadAll<CardAsset>("MyAssests/CardAssets/Decks/Beasts").ToList();
  
        deck1.Shuffle();
        deck2.Shuffle();
        deck3.Shuffle();

        switch (num)
        {
            case 1:
                return deck1;
            case 2:
                return deck2;
            case 3:
                return deck3;
            default:
                return deck1;
        }
    }

    public bool CanControlThisPlayer(AreaPosition owner)
    {
        bool PlayersTurn = (TurnManager.Instance.WhoseAction == Players[owner]);
        bool NotDrawingAnyCards = !Command.CardDrawPending();
        return Players[owner].PArea.AllowedToControlThisPlayer && Players[owner].PArea.ControlsON && PlayersTurn && NotDrawingAnyCards;
    }

    public bool CanControlThisPlayer(Player ownerPlayer)
    {
        bool PlayersTurn = (TurnManager.Instance.WhoseAction == ownerPlayer);
        bool NotDrawingAnyCards = !Command.CardDrawPending();
        return ownerPlayer.PArea.AllowedToControlThisPlayer && ownerPlayer.PArea.ControlsON && PlayersTurn && NotDrawingAnyCards;
    }

    public void EnableEndTurnButtonOnStart(Player P)
    {
        if (P == LowPlayer && CanControlThisPlayer(AreaPosition.Low))
            EndTurnButton.interactable = true;
        else
            EndTurnButton.interactable = false;
    }
}
