using UnityEngine;
using System.Collections;

public class BuffTarget : SpellEffect 
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {

        new BuffTargetCommand(target.ID, specialAmount, healthAfter: target.Health - specialAmount).AddToQueue();
        target.Health -= specialAmount;
        
        //target.Attack +=
    }
}
