using UnityEngine;
using System.Collections;

//prolly wont need it
public class UpdateManaCrystalsCommand : Command {

    private Player p;
    private int CurrentMana;
    private int MaxMana;

    public UpdateManaCrystalsCommand(Player p, int MaxMana, int CurrentMana)
    {
        this.p = p;
        this.MaxMana = MaxMana;
        this.CurrentMana = CurrentMana;
    }

    public override void StartCommandExecution()
    {
        p.PArea.ManaBar.MaxMana = MaxMana;
        p.PArea.ManaBar.CurrentMana = CurrentMana;
        CommandExecutionComplete();
    }
}
