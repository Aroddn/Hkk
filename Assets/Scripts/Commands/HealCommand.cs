using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCommand : Command
{
    private int targetID;
    private int amount;
    private int healthAfter;

    public HealCommand(int targetID, int amount, int healthAfter)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.healthAfter = healthAfter;
    }

    public override void StartCommandExecution()
    {

        GameObject target = IDHolder.GetGameObjectWithID(targetID);
        if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
        {
            // target is a hero
            target.GetComponent<PlayerPortraitVisual>().Heal(amount, healthAfter);
        }
        else
        {
            // target is a creature
            target.GetComponent<OneCreatureManager>().Heal(amount, healthAfter);
        }
        CommandExecutionComplete();
    }
}
