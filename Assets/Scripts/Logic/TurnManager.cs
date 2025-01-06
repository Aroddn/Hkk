using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

//enum to help store the last action in the game
public enum PlayerAction
{
    PlayerLowAction,
    PlayerTopAction,
    Pass,
    TurnChange
}

// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour {

    private RopeTimer timer;

    // for Singleton Pattern
    public static TurnManager Instance;

    public static List<PlayerAction> playerAction = new List<PlayerAction>();


    private Player _whoseAction;

    public Player WhoseAction
    {
        get
        {
            return _whoseAction;
        }
        set { 
            _whoseAction = value;   
        }
    }

    private Player _whoseTurn;
    public Player WhoseTurn
    {
        get
        {
            return _whoseTurn;
        }

        set
        {
            _whoseTurn = value;
            timer.StartTimer();

            GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

            TurnMaker tm = WhoseTurn.GetComponent<TurnMaker>();
            tm.OnTurnStart();
            if (tm is PlayerTurnMaker)
            {
                WhoseTurn.HighlightPlayableCards();
            }
            // remove highlights for opponent.
            WhoseTurn.otherPlayer.HighlightPlayableCards(true);
                
        }
    }

    void Awake()
    {
        Instance = this;
        timer = GetComponent<RopeTimer>();
    }

    void Start()
    {
        //OnGameStart();
    }

    public void OnGameStart()
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

            //portrait things
            p.LoadCharacterInfoFromAsset();
            // move both portraits to the center
            p.PArea.Portrait.transform.position = p.PArea.InitialPortraitPosition.transform.position;
        }

        //DOTween
        Sequence s = DOTween.Sequence();
        s.Append(Player.Players[0].PArea.Portrait.transform.DOMove(Player.Players[0].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Player.Players[1].PArea.Portrait.transform.DOMove(Player.Players[1].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.PrependInterval(3f);
        s.OnComplete(() =>
            {
                // determine who starts the game.
                int rnd = Random.Range(0,2);
                Player whoGoesFirst = Player.Players[rnd];
                Player whoGoesSecond = whoGoesFirst.otherPlayer;
         
                // draw 5 cards for first player and 5 for second player
                int initDraw = 4;
                for (int i = 0; i < initDraw; i++)
                {            
                    // second player draws a card
                    whoGoesSecond.DrawACard(true);
                    // first player draws a card
                    whoGoesFirst.DrawACard(true);
                }

                whoGoesSecond.DrawACard(true);

                new StartATurnCommand(whoGoesFirst).AddToQueue();
            });
    }

    // TEST COMMANDS
    void Update()
    {
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
        // stop timer
        timer.StopTimer();
        // send all commands in the end of current player`s turn
        WhoseTurn.OnTurnEnd();

        new StartATurnCommand(WhoseTurn.otherPlayer).AddToQueue();
    }

    public void GiveControlToOtherPlayer()
    {
        //TODO
        WhoseAction = WhoseAction.otherPlayer;
        GlobalSettings.Instance.EnableEndTurnButtonOnStart(WhoseAction);
        WhoseAction.HighlightPlayableCards();
        WhoseAction.otherPlayer.HighlightPlayableCards(true);
    }

        public void Pass()
        {
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
                if (last == PlayerAction.Pass) {

                    GlobalSettings.Instance.EndTurnButton.GetComponentInChildren<TMP_Text>().text = "End Turn";
                }
            }


        }

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

}