using UnityEngine;
using System.Collections;

public class DealDamageToTarget : SpellEffect 
{
    public override void ActivateEffect(int specialAmount = 0, int specialAmount2 = 0, ICharacter target = null)
    {
        new DealDamageCommand(target.ID, specialAmount, healthAfter: target.Health - specialAmount).AddToQueue();
        //target.Health -= specialAmount;
        target.ChangeHealth(-specialAmount,false);
    }
}
