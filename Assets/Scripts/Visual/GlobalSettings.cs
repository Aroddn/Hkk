using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System;
using Unity.Collections.LowLevel.Unsafe;
using System.Linq;

public class GlobalSettings: MonoBehaviour 
{
    [Header("Players")]
    public Player TopPlayer;
    public Player LowPlayer;
    [Header("Colors")]
    public Color32 CardBodyStandardColor;
    public Color32 CardRibbonsStandardColor;
    public Color32 CardGlowColor;
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

    // SINGLETON
    public static GlobalSettings Instance;

    void Awake()
    {
        Players.Add(AreaPosition.Top, TopPlayer);
        Players.Add(AreaPosition.Low, LowPlayer);
        Instance = this;


        AssignDeckAndCharacter(LowPlayer, BattleStartInfo.SelectedDeck);
        AssignDeckAndCharacter(TopPlayer, null);
    }

    private void AssignDeckAndCharacter(Player player, DeckInfo deckInfo)
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

        string[] angels = AssetDatabase.FindAssets("t:CardAsset", new[] { "Assets/Resources/SO Assets/Decks/Angels" });
        string[] fire = AssetDatabase.FindAssets("t:CardAsset", new[] { "Assets/Resources/SO Assets/Decks/FireMagic" });
        string[] beasts = AssetDatabase.FindAssets("t:CardAsset", new[] { "Assets/Resources/SO Assets/Decks/Beasts" });

        foreach (string angel in angels)
        {
            string path = AssetDatabase.GUIDToAssetPath(angel);
            CardAsset card = AssetDatabase.LoadAssetAtPath<CardAsset>(path);
            deck1.AddRange(Enumerable.Repeat(card, 3));

        }

        foreach (string f in fire)
        {
            string path = AssetDatabase.GUIDToAssetPath(f);
            CardAsset card = AssetDatabase.LoadAssetAtPath<CardAsset>(path);
            deck2.AddRange(Enumerable.Repeat(card, 3));

        }

        foreach (string b in beasts)
        {
            string path = AssetDatabase.GUIDToAssetPath(b);
            CardAsset card = AssetDatabase.LoadAssetAtPath<CardAsset>(path);
            deck3.AddRange(Enumerable.Repeat(card, 3));

        }

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
        //bool PlayersTurn = (TurnManager.Instance.whoseTurn == Players[owner]);
        bool PlayersTurn = (TurnManager.Instance.WhoseAction == Players[owner]);
        //Debug.Log(TurnManager.Instance.WhoseAction);
        bool NotDrawingAnyCards = !Command.CardDrawPending();
        return Players[owner].PArea.AllowedToControlThisPlayer && Players[owner].PArea.ControlsON && PlayersTurn && NotDrawingAnyCards;
    }

    public bool CanControlThisPlayer(Player ownerPlayer)
    {
        //bool PlayersTurn = (TurnManager.Instance.whoseTurn == ownerPlayer);
        bool PlayersTurn = (TurnManager.Instance.WhoseAction == ownerPlayer);
        bool NotDrawingAnyCards = !Command.CardDrawPending();
        return ownerPlayer.PArea.AllowedToControlThisPlayer && ownerPlayer.PArea.ControlsON && PlayersTurn && NotDrawingAnyCards;
    }

    public void EnableEndTurnButtonOnStart(Player P)
    {
        if (P == LowPlayer && CanControlThisPlayer(AreaPosition.Low) ||
            P == TopPlayer && CanControlThisPlayer(AreaPosition.Top))
            EndTurnButton.interactable = true;
        else
            EndTurnButton.interactable = false;
            
    }
}
