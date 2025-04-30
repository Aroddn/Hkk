using UnityEngine;
using System.Collections;
using Mirror;


//command to queu up when a creatue dies and need to get removed from the table
public class CreatureDeathCommand : Command
{
    private Player p;
    private int DeadCreatureID;
    private bool sacrifice;
    //in case deathrattle effects are implemented
    //private CreatureEffect effect;

    public CreatureDeathCommand(int CreatureID, Player p, bool sacrifice){
        this.p = p;
        this.DeadCreatureID = CreatureID;
        this.sacrifice = sacrifice;

        //this.effect = effect;
    }
    public override void StartCommandExecution(){
        //RemoeCreatureWithID called on the server to remove creature on the board of the player
        p.PArea.tableVisual.RemoveCreatureWithID(DeadCreatureID,p,sacrifice);
    }
}
