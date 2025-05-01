using UnityEngine;
using System.Collections;

public class SendTargetToGraveyard : SpellEffect 
{
    public override void ActivateEffect(int specialAmount = 0, int specialAmount2 = 0, ICharacter target = null)
    {
        target.Die(false);
    }
}
