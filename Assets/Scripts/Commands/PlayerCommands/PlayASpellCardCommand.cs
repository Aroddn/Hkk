using UnityEngine;
using System.Collections;

//play a spellcard with no target
public class PlayASpellCardCommand: Command
{
    private CardLogic card;
    private Player p;

    public PlayASpellCardCommand(Player p, CardLogic card){
        this.card = card;
        this.p = p;
    }

    public override void StartCommandExecution(){
        p.PArea.handVisual.PlayASpellFromHand(card.UniqueCardID);
    }
}
