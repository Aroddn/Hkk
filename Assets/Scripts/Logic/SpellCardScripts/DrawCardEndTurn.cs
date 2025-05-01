using UnityEngine;
using System.Collections;

public class DrawCardEndTurn : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, int specialAmount2 = 0, ICharacter target = null)
    {
        TurnManager.Instance.WhoseAction.EndTurnEvent += DrawCardsAtEndOfTurn;
        //Debug.Log($"Activated DrawCardEffect for {TurnManager.Instance.WhoseAction.PlayerID}. Cards to draw: {specialAmount}");
    }

    private void DrawCardsAtEndOfTurn()
    {
        var player = TurnManager.Instance.WhoseAction;

        new DrawACardCommand(player.hand.CardsInHand[0], player, true, fromDeck: true).AddToQueue();

        //Debug.Log($"Player {player.PlayerID} draws a card at the end of their turn.");

        DeactivateEffect();
    }

    public void DeactivateEffect()
    {
        TurnManager.Instance.WhoseAction.EndTurnEvent -= DrawCardsAtEndOfTurn;
        //Debug.Log($"Unsubscribed DrawCardEffect for {TurnManager.Instance.WhoseAction.PlayerID}.");
    }

}
