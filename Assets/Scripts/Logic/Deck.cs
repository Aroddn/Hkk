using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using Mirror;

[System.Serializable]
public class Deck : NetworkBehaviour
{
    public List<CardAsset> cards = new List<CardAsset>();
}
