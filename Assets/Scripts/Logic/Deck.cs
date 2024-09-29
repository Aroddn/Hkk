using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public class Deck : MonoBehaviour {

    public List<CardAsset> cards;// = new List<CardAsset>()


    void Awake()
    {    
        cards.Shuffle();
    }
	
}
