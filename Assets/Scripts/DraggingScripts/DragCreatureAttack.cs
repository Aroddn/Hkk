using UnityEngine;
using System.Collections;

//dragging script attached to the Creature prefab

public class DragCreatureAttack : DraggingActions {

    private SpriteRenderer sr;
    private LineRenderer lr;
    private WhereIsTheCardOrCreature whereIsThisCreature;
    private Transform triangle;
    private SpriteRenderer triangleSR;
    private GameObject Target;
    private OneCreatureManager manager;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();
        manager = GetComponentInParent<OneCreatureManager>();
        whereIsThisCreature = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override bool CanDrag
    {
        get
        {
            return base.CanDrag && manager.CanAttackNow;
        }
    }

    public override void OnStartDrag()
    {
        whereIsThisCreature.VisualState = VisualStates.Dragging;
        sr.enabled = true;
        lr.enabled = true;
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction*2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            //draws a line between the creature and the target
            lr.SetPositions(new Vector3[]{ transform.parent.position, transform.position - direction*2.3f });
            lr.enabled = true;

            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f*direction;

            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            lr.enabled = false;
            triangleSR.enabled = false;
        }
            
    }

    public override void OnEndDrag()
    {
        Target = null;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
            direction: (-Camera.main.transform.position + this.transform.position).normalized, 
            maxDistance: 30f) ;

        foreach (RaycastHit h in hits)
        { 
            if ((h.transform.tag == "TopPlayer" && this.tag == "LowCreature") ||
                (h.transform.tag == "LowPlayer" && this.tag == "TopCreature"))
            {
                //target is a player, but not our own
                Target = h.transform.gameObject;
            }
            else if ((h.transform.tag == "TopCreature" && this.tag == "LowCreature") ||
                    (h.transform.tag == "LowCreature" && this.tag == "TopCreature"))
            {
                //target is a creature, but not on of our own
                Target = h.transform.parent.gameObject;
            }
               
        }

        bool targetValid = false;

        if (Target != null)
        {
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
            {
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].AttackPlayer();
                targetValid = true;
            }
            else if (CreatureLogic.CreaturesCreatedThisGame[targetID] != null)
            {
                targetValid = true;
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].AttackCreatureWithID(targetID);
            }

        }

        if (!targetValid)
        {
            if (tag.Contains("Low"))
                whereIsThisCreature.VisualState = VisualStates.LowTable;
            else
                whereIsThisCreature.VisualState = VisualStates.TopTable;
            whereIsThisCreature.SetTableSortingOrder();
        }
        transform.localPosition = Vector3.zero;
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

    }

    protected override bool DragSuccessful()
    {
        return true;
    }

    public override void OnCancelDrag()
    {

    }
}
