using UnityEngine;
using System.Collections;

public class UpdateBonesCommand : Command {

    private Player p;
    private int Bones;

    public UpdateBonesCommand(Player p, int Bones)
    {
        this.p = p;
        this.Bones = Bones;
    }

    public override void StartCommandExecution()
    {
        p.PArea.Bones.TotalBones = Bones;
        CommandExecutionComplete();
    }
}
