using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//main coammand to implement queue system for other commands/effects
//very crucial for gameplay
//we dont want player actions to overlap

public class Command
{
    public static Queue<Command> CommandQueue = new Queue<Command>();
    public static bool playingQueue = false;

    public virtual void StartCommandExecution() { }

    //checks if we have a pending card draw
    public static bool CardDrawPending()
    {
        foreach (Command c in CommandQueue){
            if (c is DrawACardCommand)
                return true;
        }
        return false;
    }

    //command executes instantly
    public static void CommandExecutionComplete(){
        if (CommandQueue.Count > 0)
            ExecuteFirstCommandFromQueue();
        else
            playingQueue = false;
        if (TurnManager.Instance.WhoseTurn != null)
            TurnManager.Instance.WhoseTurn.PlayableCardHighlighter();
    }
    public virtual void AddToQueue(){
        CommandQueue.Enqueue(this);
        if (!playingQueue)
            ExecuteFirstCommandFromQueue();
    }

    //executes the first command in the queu
    public static void ExecuteFirstCommandFromQueue(){
        playingQueue = true;
        CommandQueue.Dequeue().StartCommandExecution();
    }
}
