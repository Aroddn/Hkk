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
    //public CardAsset CoinCard;
    public GameObject GameOverCanvas;
    //public Sprite HeroPowerCrossMark;

    public Dictionary<AreaPosition, Player> Players = new Dictionary<AreaPosition, Player>();

    // SINGLETON
    public static GlobalSettings Instance;

    void Awake()
    {
        Players.Add(AreaPosition.Top, TopPlayer);
        Players.Add(AreaPosition.Low, LowPlayer);
        Instance = this;

        List<CardAsset> deck = new List<CardAsset>();
        List<CardAsset> deck2 = new List<CardAsset>();

        string[] angels = AssetDatabase.FindAssets("t:CardAsset", new[] { "Assets/SO Assets/Decks/Angels" });
        string[] fire = AssetDatabase.FindAssets("t:CardAsset", new[] { "Assets/SO Assets/Decks/FireMagic" });
        string[] beasts = AssetDatabase.FindAssets("t:CardAsset", new[] { "Assets/SO Assets/Decks/Beasts" });

        foreach (string angel in angels)
        {
            string path = AssetDatabase.GUIDToAssetPath(angel);
            CardAsset card = AssetDatabase.LoadAssetAtPath<CardAsset>(path);
            deck.AddRange(Enumerable.Repeat(card, 3));

        }
        //deck.Shuffle();
        Players[AreaPosition.Top].deck.cards = deck;

        foreach (string f in fire)
        {
            string path = AssetDatabase.GUIDToAssetPath(f);
            CardAsset card = AssetDatabase.LoadAssetAtPath<CardAsset>(path);
            deck2.AddRange(Enumerable.Repeat(card, 3));

        }
        deck2.Shuffle();

        Players[AreaPosition.Low].deck.cards = deck2;
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
