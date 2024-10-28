using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using Unity.VisualScripting;

public class DrawCard : CreatureEffect
{  
    public DrawCard(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    public override void WhenACreatureIsPlayed()
    {    
        new DrawACardCommand(owner.hand.CardsInHand[0], owner, true, fromDeck: true).AddToQueue();
        new DrawACardCommand(owner.otherPlayer.hand.CardsInHand[0], owner.otherPlayer, true, fromDeck: true).AddToQueue();
    }
}
