using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Xml;

//this is where we drag the creature over the table and "drop it" on it

public class DragCreatureOnTable : DraggingActions {

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private VisualStates tempState;
    private OneCardManager manager;

    public override bool CanDrag
    {
        get
        {
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        manager = GetComponent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;
        tempState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

    }

    public override void OnDraggingInUpdate()
    {

    }

    public override void OnEndDrag()
    {
        if (DragSuccessful())
        {
            int tablePos = playerOwner.PArea.tableVisual.TablePosForNewCreature(Camera.main.ScreenToWorldPoint(
               new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x);
            playerOwner.CmdPlayCreature(GetComponent<IDHolder>().UniqueID, tablePos);

        }
        else
        {
            whereIsCard.SetHandSortingOrder();
            whereIsCard.VisualState = tempState;
            HandVisual PlayerHand = playerOwner.PArea.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        }
    }

    protected override bool DragSuccessful()
    {
        bool TableNotFull = (playerOwner.table.CreaturesOnTable.Count < 8);
        bool ownTurn = TurnManager.Instance.WhoseAction == TurnManager.Instance.WhoseTurn;
        return TableVisual.CursorOverSomeTable && TableNotFull && ownTurn;
    }

    public override void OnCancelDrag()
    {

    }
}
