using UnityEngine;
using System.Collections;
using Mirror;

//queus up to command to initiate animation for attacking target
public class CreatureAttackCommand : Command 
{
    private int TargetID;
    private int AttackerID;
    private int AttackerHealthAfter;
    private int TargetHealthAfter;
    private int DamageTakenByAttacker;
    private int DamageTakenByTarget;

    public CreatureAttackCommand(int targetID, int attackerID, int damageTakenByAttacker, int damageTakenByTarget, int attackerHealthAfter, int targetHealthAfter){
        this.TargetID = targetID;
        this.AttackerID = attackerID;
        this.AttackerHealthAfter = attackerHealthAfter;
        this.TargetHealthAfter = targetHealthAfter;
        this.DamageTakenByTarget = damageTakenByTarget;
        this.DamageTakenByAttacker = damageTakenByAttacker;
    }

    public override void StartCommandExecution(){
        //founds the CreaturePrefab on board which started the attack
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerID);

        //AttacTarget is called on the server and gets replicated to all clients
        Attacker.GetComponent<CreatureAttackVisual>().AttackTarget(TargetID, DamageTakenByTarget, DamageTakenByAttacker, AttackerHealthAfter, TargetHealthAfter);
    }
}
