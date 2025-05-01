using UnityEngine;
using System.Collections;

public class BuffTarget : SpellEffect 
{
    public override void ActivateEffect(int specialAmount = 0, int specialAmount2 = 0, ICharacter target = null)
    {

        new BuffTargetCommand(target.ID,attackAfter: target.Attack + specialAmount,healthAfter: target.Health + specialAmount).AddToQueue();

        target.Attack += specialAmount;
        target.ChangeHealth(specialAmount2, false);
    }
}
