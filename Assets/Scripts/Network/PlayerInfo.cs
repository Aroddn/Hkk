using System;
using UnityEngine;
using Mirror;

[Serializable]
public partial struct PlayerInfo
{
    public GameObject player;

    public PlayerInfo(GameObject player)
    {
        this.player = player;
    }

    public Player data
    {
        get
        {
            // Return ScriptableItem from our cached list, based on the card's uniqueID.
            return player.GetComponentInChildren<Player>();
        }
    }

    // Player's username
    public string username => data.username;
    //public Sprite portrait => data.portrait;

    // Player health and mana
    public int health => data.Health;
    public int mana => data.CurrentMana;

    // Cardback image
    //public Sprite cardback => data.cardback;

    // Card count for UI
    public int handCount => data.hand.CardsInHand.Count;
    public int deckCount => data.deck.cards.Count;
    public int graveCount => data.graveYard.cards.Count;
    public int voidCount => data.voiid.cards.Count;
}

// Card List
public class SyncListPlayerInfo : SyncList<PlayerInfo> { }
