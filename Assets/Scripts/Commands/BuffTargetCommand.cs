using UnityEngine;
using System.Collections;

public class BuffTargetCommand : Command {

    private int targetID;
    private int attackAfter;
    private int healthAfter;

    public BuffTargetCommand( int targetID,int attackAfter,int healthAfter)
    {
        this.targetID = targetID;
        this.attackAfter = attackAfter;
        this.healthAfter = healthAfter;
    }

    public override void StartCommandExecution()
    {

        GameObject target = IDHolder.GetGameObjectWithID(targetID);
        if (!(targetID == GlobalSettings.Instance.LowPlayer.PlayerID) && !(targetID == GlobalSettings.Instance.TopPlayer.PlayerID))
        {
            target.GetComponent<OneCreatureManager>().Buff(attackAfter, healthAfter);
        }

        CommandExecutionComplete();
    }
}
