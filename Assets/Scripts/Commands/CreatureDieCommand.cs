using UnityEngine;
using System.Collections;
using Mirror;

public class CreatureDieCommand : Command
{
    private Player p;
    private int DeadCreatureID;
    private bool sacrifice;
    //private CreatureEffect effect;

    public CreatureDieCommand(int CreatureID, Player p, bool sacrifice)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
        this.sacrifice = sacrifice;
        //this.effect = effect;
    }
    public override void StartCommandExecution()
    {
        p.PArea.tableVisual.RemoveCreatureWithID(DeadCreatureID,p,sacrifice);
    }
}
