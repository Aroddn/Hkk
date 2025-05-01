using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Mirror;
using Telepathy;

//enum to help store the last action in the game
public enum PlayerAction
{
    PlayerLowAction,
    PlayerTopAction,
    Pass,
    TurnChange
}

public class TurnManager : NetworkBehaviour {

    private RopeTimer timer;
    public static TurnManager Instance;
    public static List<PlayerAction> playerAction = new List<PlayerAction>();
    private Player _whoseAction;

    public Player WhoseAction{
        get {return _whoseAction;}
        set {_whoseAction = value;}
    }

    private Player _whoseTurn;

    public Player WhoseTurn{
        get{return _whoseTurn;}

        set
        {
            _whoseTurn = value;
            timer.StartTimer();

            GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

            TurnMaker tm = WhoseTurn.GetComponent<TurnMaker>();
            tm.OnTurnStart();
            if (tm is PlayerTurnMaker)
            {
                WhoseTurn.PlayableCardHighlighter();
            }
            WhoseTurn.otherPlayer.PlayableCardHighlighter(true);
        }
    }



    void Awake()
    {
        Instance = this;
        timer = GetComponent<RopeTimer>();
    }

    public void OnGameStart()
    {
        int rnd = Random.Range(0, 2);
        RpcSetWhoGoesFirst(rnd);
    }

    [ClientRpc]
    public void RpcSetWhoGoesFirst(int playerIndex)
    {
        CardLogic.CardsCreatedThisGame.Clear();
        CreatureLogic.CreaturesCreatedThisGame.Clear();

        foreach (Player p in Player.Players)
        {
            p.PArea.Portrait.gameObject.SetActive(true);
            p.MaxMana = 20;
            p.CurrentMana = 0;
            p.TotalBones = 0;
            p.PArea.PDeck.CardsInDeck = p.deck.cards.Count;
            p.TransmitInfoAboutPlayerToVisual();
            p.LoadCharacterInfoFromAsset();
            p.PArea.Portrait.transform.position = p.PArea.InitialPortraitPosition.transform.position;
        }

        Sequence s = DOTween.Sequence();
        s.Append(Player.Players[0].PArea.Portrait.transform.DOMove(Player.Players[0].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Player.Players[1].PArea.Portrait.transform.DOMove(Player.Players[1].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.PrependInterval(3f);
        s.OnComplete(() =>
        {
            Player whoGoesFirst = Player.Players[playerIndex];
            Player whoGoesSecond = whoGoesFirst.otherPlayer;

            int initDraw = 4;
            for (int i = 0; i < initDraw; i++)
            {
                whoGoesSecond.DrawACard(true);
                whoGoesFirst.DrawACard(true);
            }

            whoGoesSecond.DrawACard(true);
            new StartANewTurnCommand(whoGoesFirst).AddToQueue();
        });
    }

    //Commands for testing
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && NetworkServer.active && isServer)
        {
            // Only start if exactly 2 players are connected
            if (NetworkServer.connections.Count == 2)
            {
                OnGameStart();
            }
            else
            {
                Debug.Log("Waiting for 2 players. Current: " + NetworkServer.connections.Count);
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //    EndTurn();

        //if (Input.GetKeyDown(KeyCode.F))
        //    WhoseTurn.DrawACard(true);

        //if (Input.GetKeyDown(KeyCode.M))
        //    WhoseTurn.ManaLeft--;

        //if (Input.GetKeyDown(KeyCode.D))
        //    WhoseTurn.DrawACard(); 
    }

    public void EndTurn()
    {
        timer.StopTimer();
        WhoseTurn.OnTurnEnd();
        new StartANewTurnCommand(WhoseTurn.otherPlayer).AddToQueue();
    }

    public void GiveControlToOtherPlayer(){
        WhoseAction = WhoseAction.otherPlayer;
        GlobalSettings.Instance.EnableEndTurnButtonOnStart(WhoseAction);
        WhoseAction.PlayableCardHighlighter();
        WhoseAction.otherPlayer.PlayableCardHighlighter(true);
    }

    //logic for the Pass button
    [ClientRpc]
    public void RcpPass(){
        playerAction.Add(PlayerAction.Pass);
        PlayerAction last = playerAction[playerAction.Count - 1];

        if (playerAction.Count > 1)
        {
            PlayerAction secondLast = playerAction[playerAction.Count - 2];

            //whoseTurn changes
            if (secondLast == PlayerAction.Pass && last == PlayerAction.Pass)
            {
                playerAction.Add(PlayerAction.TurnChange);
                EndTurn();
                GlobalSettings.Instance.EndTurnButton.GetComponentInChildren<TMP_Text>().text = "Pass";
            }
            else
            {

                if (last == PlayerAction.Pass && secondLast == PlayerAction.TurnChange ||
                    last == PlayerAction.Pass && secondLast == PlayerAction.PlayerLowAction ||
                    last == PlayerAction.Pass && secondLast == PlayerAction.PlayerTopAction)
                {
                    //give other player control
                    //so whoseAction changes
                    GiveControlToOtherPlayer();
                    GlobalSettings.Instance.EndTurnButton.GetComponentInChildren<TMP_Text>().text = "End Turn";
                }
            }
        }
        else
        {
            if (last == PlayerAction.Pass){
                GiveControlToOtherPlayer();
                GlobalSettings.Instance.EndTurnButton.GetComponentInChildren<TMP_Text>().text = "End Turn";
            }
        }
    }

    //the main Pass() command gets executed on the server then calls RcpPass to replicate it to all clients
    [Command(requiresAuthority = false)]
    public void Pass()
    {
        RcpPass();
    }

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

}