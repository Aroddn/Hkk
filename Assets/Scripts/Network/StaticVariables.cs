using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVariables : MonoBehaviour
{
    public static CharacterAsset character;
    public static List<CardAsset> cards;
    public static string deckName = "Default Deck";
    public static string charAssetName = "Adventurer";
    public static List<string> cardNames = new List<string> {};
}