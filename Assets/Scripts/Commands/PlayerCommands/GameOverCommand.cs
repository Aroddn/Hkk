using UnityEngine;
using System.Collections;


//command to make the loser who lost explode
public class GameOverCommand : Command{

    private Player loser;

    public GameOverCommand(Player loser){
        this.loser = loser;
    }

    public override void StartCommandExecution(){
        loser.PArea.Portrait.Explode();
    }
}
