using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public PlayerArea LowPlayerArea;
    public PlayerArea TopPlayerArea;
    public TurnManager TurnManager;

    [ClientRpc]
    public void RpcDieCreature(GameObject creatureObj, bool sacrifice)
    {
        if (creatureObj == null) return;

        CreatureLogic creature = creatureObj.GetComponent<CreatureLogic>();
        if (creature != null)
        {
            creature.Die(sacrifice);
        }
    }


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