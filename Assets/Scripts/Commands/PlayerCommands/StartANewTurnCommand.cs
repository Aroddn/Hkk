using UnityEngine;
using System.Collections;

public class StartANewTurnCommand : Command {

    private Player p;

    public StartANewTurnCommand(Player p){
        this.p = p;
    }

    public override void StartCommandExecution(){
        TurnManager.Instance.WhoseAction = p;
        TurnManager.Instance.WhoseTurn = p;
        CommandExecutionComplete();
    }
}
