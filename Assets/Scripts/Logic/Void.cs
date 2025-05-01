using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using Mirror;

//hold the CardAsset of cards that are sent to the "Void"

public class Void : NetworkBehaviour {

    public List<CardAsset> cards;

    void Awake(){}
	
}
