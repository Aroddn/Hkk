using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public PlayerArea LowPlayerArea;
    public PlayerArea TopPlayerArea;
    public TurnManager TurnManager;

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
    }
}