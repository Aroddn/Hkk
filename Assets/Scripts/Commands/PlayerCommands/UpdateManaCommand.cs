using UnityEngine;
using System.Collections;

//command to change a players mana pool
//it could change their current mana but their max as well
public class UpdateManaCommand : Command {
    private Player p;
    private int CurrentMana;
    private int MaxMana;

    public UpdateManaCommand(Player p, int MaxMana, int CurrentMana){
        this.p = p;
        this.MaxMana = MaxMana;
        this.CurrentMana = CurrentMana;
    }

    public override void StartCommandExecution(){
        p.PArea.ManaBar.MaxMana = MaxMana;
        p.PArea.ManaBar.CurrentMana = CurrentMana;
        CommandExecutionComplete();
    }
}
