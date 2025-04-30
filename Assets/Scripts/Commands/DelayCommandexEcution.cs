using UnityEngine;
using System.Collections;
using DG.Tweening;

//delays a command being completed by a certain amount of time
public class DelayCommandExecution : Command 
{
    float delay;
    public DelayCommandExecution(float timeToWait)
    {
        delay = timeToWait;    
    }
    public override void StartCommandExecution(){
        Sequence s = DOTween.Sequence();
        s.PrependInterval(delay);
        s.OnComplete(Command.CommandExecutionComplete);
    }
}
