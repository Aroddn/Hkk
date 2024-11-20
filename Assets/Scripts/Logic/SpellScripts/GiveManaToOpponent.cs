using UnityEngine;
using System.Collections;

public class GiveManaToOpponent: SpellEffect 
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        TurnManager.Instance.WhoseAction.otherPlayer.GetBonusMana(specialAmount);
    }
}
