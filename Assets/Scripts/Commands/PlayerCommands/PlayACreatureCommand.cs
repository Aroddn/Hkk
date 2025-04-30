using UnityEngine;
using System.Collections;


//puts a creature on the table
public class PlayACreatureCommand : Command
{
    private CardLogic cl;
    private int tablePos;
    private Player p;
    private CreatureLogic creatureLogic;
    private GameObject creature;

    public PlayACreatureCommand(CardLogic cl, Player p, int tablePos, CreatureLogic creatureLogic, GameObject creature){
        this.p = p;
        this.cl = cl;
        this.tablePos = tablePos;
        this.creatureLogic = creatureLogic;
        this.creature = creature;
    }

    public override void StartCommandExecution(){
        HandVisual PlayerHand = p.PArea.handVisual;
        GameObject card = IDHolder.GetGameObjectWithID(cl.UniqueCardID);


        //cards gets removed from hand
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);


        HoverPreview.PreviewsAllowed = true;

        //AddCreatureAtIndex is called on the server and gets replicated for all clients+
        p.PArea.tableVisual.AddCreatureAtIndex(cl.ca, creatureLogic, tablePos,creature);
    }
}
