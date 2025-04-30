using UnityEngine;
using System.Collections;
using DG.Tweening;

//main dragging script attached to cards we can move

public class Draggable : MonoBehaviour {
    public enum StartDragBehavior
    {
        OnMouseDown, InAwake
    }

    public enum EndDragBehavior
    {
        OnMouseUp, OnMouseDown
    }

    public StartDragBehavior HowToStart = StartDragBehavior.OnMouseDown;
    public EndDragBehavior HowToEnd = EndDragBehavior.OnMouseUp;
    private bool dragging = false;
    private Vector3 pointerDisplacement;
    private float zDisplacement;
    private DraggingActions da;
    private static Draggable _draggingThis;

    public static Draggable DraggingThis
    {
        get{ return _draggingThis;}
    }

    void Awake()
    {
        da = GetComponent<DraggingActions>();
    }

    void OnMouseDown()
    {
        if (da != null && da.CanDrag && HowToStart == StartDragBehavior.OnMouseDown)
        {
            StartDragging();
        }

        if (dragging && HowToEnd == EndDragBehavior.OnMouseDown)
        {
            dragging = false;
            HoverPreview.PreviewsAllowed = true;
            _draggingThis = null;
            da.OnEndDrag();
        }
    }

    void OnMouseUp()
    {
        if (dragging && HowToEnd == EndDragBehavior.OnMouseUp)
        {
            dragging = false;
            HoverPreview.PreviewsAllowed = true;
            _draggingThis = null;
            da.OnEndDrag();
        }
    }

    void Update ()
    {
        if (dragging)
        { 
            Vector3 mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);   
            da.OnDraggingInUpdate();
        }
    }
    public void StartDragging()
    {
        dragging = true;
        HoverPreview.PreviewsAllowed = false;
        _draggingThis = this;
        da.OnStartDrag();
        zDisplacement = -Camera.main.transform.position.z + transform.position.z;
        pointerDisplacement = -transform.position + MouseInWorldCoords();
    }

    public void CancelDrag()
    {
        if (dragging)
        {
            dragging = false;
            HoverPreview.PreviewsAllowed = true;
            _draggingThis = null;
            da.OnCancelDrag();
        }
    }

    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
        
}
