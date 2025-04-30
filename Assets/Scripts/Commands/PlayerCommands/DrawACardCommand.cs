using UnityEngine;
using System.Collections;
using Mirror;

//command for players to draw cards
public class DrawACardCommand : Command {

    private bool fast;
    private bool fromDeck;
    private CardLogic cl;
    private Player p;

    public DrawACardCommand(CardLogic cl, Player p, bool fast, bool fromDeck){        
        this.cl = cl;
        this.p = p;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }

    public override void StartCommandExecution(){
        p.PArea.PDeck.CardsInDeck--;
        p.PArea.handVisual.GivePlayerACard(cl.ca, cl.UniqueCardID, fast, fromDeck);
    }
}
