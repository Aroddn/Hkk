using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Enum to know where this object is
public enum VisualStates
{
    Transition,
    LowHand, 
    TopHand,
    LowTable,
    TopTable,
    Dragging,
    LowTableGuard,
    TopTableGuard
}

public class WhereIsTheCardOrCreature : MonoBehaviour {
    private HoverPreview hover;
    private Canvas canvas;
    private int TopSortingOrder = 500;

    private int slot = -1;
    public int Slot
    {
        get{ return slot;}

        set
        {
            slot = value;
        }
    }

    private VisualStates state;
    public VisualStates VisualState
    {
        get{ return state; }  

        set
        {
            state = value;
            switch (state)
            {
                case VisualStates.LowHand:
                    hover.ThisPreviewEnabled = true;
                    break;
                case VisualStates.LowTable:
                case VisualStates.TopTable:
                    hover.ThisPreviewEnabled = true;
                    break;
                case VisualStates.Transition:
                    hover.ThisPreviewEnabled = false;
                    break;
                case VisualStates.Dragging:
                    hover.ThisPreviewEnabled = false;
                    break;
                case VisualStates.TopHand:
                    hover.ThisPreviewEnabled = false;
                    break;
            }
        }
    }

    void Awake()
    {
        hover = GetComponent<HoverPreview>();
        if (hover == null)
            hover = GetComponentInChildren<HoverPreview>();
        canvas = GetComponentInChildren<Canvas>();
    }

    public void BringToFront()
    {
        canvas.sortingOrder = TopSortingOrder;
        canvas.sortingLayerName = "AboveEverything";
    }

    public void SetHandSortingOrder()
    {
        if (slot != -1)
            canvas.sortingOrder = HandSortingOrder(slot);
        canvas.sortingLayerName = "Cards";
    }

    public void SetTableSortingOrder()
    {
        canvas.sortingOrder = 0;
        canvas.sortingLayerName = "Creatures";
    }

    private int HandSortingOrder(int placeInHand)
    {
        return ((placeInHand + 1) * 10); 
    }
}
