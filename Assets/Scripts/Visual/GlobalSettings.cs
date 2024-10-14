using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System;
using Unity.Collections.LowLevel.Unsafe;

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

        string[] angels = AssetDatabase.FindAssets("t:CardAsset", new[] { "Assets/SO Assets/Decks/Angels" });
        string[] beasts = AssetDatabase.FindAssets("t:CardAsset", new[] { "Assets/SO Assets/Decks/FireMagic" });
        string[] fire = AssetDatabase.FindAssets("t:CardAsset", new[] { "Assets/SO Assets/Decks/Beasts" });

        foreach (string angel in angels)
        {
            string path = AssetDatabase.GUIDToAssetPath(angel);
            CardAsset card = AssetDatabase.LoadAssetAtPath<CardAsset>(path);
            deck.Add(card);
            
        }
        Players[AreaPosition.Low].deck.cards = deck;

        deck.Clear();

        foreach (string f in fire)
        {
            string path = AssetDatabase.GUIDToAssetPath(f);
            CardAsset card = AssetDatabase.LoadAssetAtPath<CardAsset>(path);
            deck.Add(card);

        }

        Players[AreaPosition.Top].deck.cards = deck;
    }

    public bool CanControlThisPlayer(AreaPosition owner)
    {
        //bool PlayersTurn = (TurnManager.Instance.whoseTurn == Players[owner]);
        bool PlayersTurn = (TurnManager.Instance.WhoseAction == Players[owner]);
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
