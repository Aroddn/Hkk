using UnityEngine;
using System.Collections;

public class HealTarget : SpellEffect 
{
    public override void ActivateEffect(int specialAmount = 0, int specialAmount2 = 0, ICharacter target = null)
    {
        new HealCommand(target.ID, specialAmount, healthAfter: target.Health + specialAmount).AddToQueue();  
        target.ChangeHealth(specialAmount,true);
        //TurnManager.Instance.WhoseAction.DrawACard();
    }
}
