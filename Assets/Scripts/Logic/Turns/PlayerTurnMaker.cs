using UnityEngine;
using System.Collections;

public class PlayerTurnMaker : TurnMaker 
{
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (p.PArea.owner == AreaPosition.Low)
        {
            new ShowMessageCommand("Your Turn!", 2.0f).AddToQueue();
        }
        else if (p.PArea.owner == AreaPosition.Top)
        {
            new ShowMessageCommand("Enemy's turn!", 2.0f).AddToQueue();
        }

        p.DrawACard();
    }
}
