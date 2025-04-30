using UnityEngine;
using System.Collections;

//similar to UpdateManaCommand but it changes a player Bone count
public class UpdateBonesCommand : Command {

    private Player p;
    private int Bones;

    public UpdateBonesCommand(Player p, int Bones){
        this.p = p;
        this.Bones = Bones;
    }

    public override void StartCommandExecution(){
        p.PArea.Bones.TotalBones = Bones;
        CommandExecutionComplete();
    }
}
