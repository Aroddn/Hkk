using UnityEngine;
using System.Collections;

public class DamageAllOpponentCreatures : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, int specialAmount2 = 0, ICharacter target = null)
    {
        CreatureLogic[] CreaturesToDamage = TurnManager.Instance.WhoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
            //cl.Health -= specialAmount;
            cl.ChangeHealth(-specialAmount, false);
        }
    }
}
