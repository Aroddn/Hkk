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
}