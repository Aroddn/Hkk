using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using System.Linq;
using Unity.VisualScripting;

public class TableVisual : NetworkBehaviour
{
    // PUBLIC FIELDS

    // an enum that mark to which caracter this table belongs. The alues are - Top or Low
    public AreaPosition owner;

    // a referense to a game object that marks positions where we should put new Creatures
    public SameDistanceChildren slots;

    // PRIVATE FIELDS

    // list of all the creature cards on the table as GameObjects
    public List<GameObject> CreaturesOnTable = new List<GameObject>();

    // are we hovering over this table`s collider with a mouse
    private bool cursorOverThisTable = false;

    // A 3D collider attached to this game object
    private BoxCollider col;

    // PROPERTIES

    // returns true if we are hovering over any player`s table collider
    public static bool CursorOverSomeTable
    {
        get
        {
            TableVisual[] bothTables = GameObject.FindObjectsOfType<TableVisual>();
            return (bothTables[0].CursorOverThisTable || bothTables[1].CursorOverThisTable);
        }
    }

    // returns true only if we are hovering over this table`s collider
    public bool CursorOverThisTable
    {
        get{ return cursorOverThisTable; }
    }

    // MONOBEHAVIOUR SCRIPTS (mouse over collider detection)
    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    // CURSOR/MOUSE DETECTION
    void Update()
    {
        // we need to Raycast because OnMouseEnter, etc reacts to colliders on cards and cards "cover" the table
        RaycastHit[] hits;
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);
        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                passedThroughTableCollider = true;
        }
        cursorOverThisTable = passedThroughTableCollider;
    }


    public void AddCreatureAtIndex(CardAsset ca, CreatureLogic creatureLogic, int index, GameObject creature)
    {
        //GameObject creature = GameObject.Instantiate(GlobalSettings.Instance.CreaturePrefab, slots.Children[index].transform.position, Quaternion.identity) as GameObject;
        creature.transform.position = slots.Children[index].transform.position;
        // apply the look from CardAsset
        OneCreatureManager manager = creature.GetComponent<OneCreatureManager>();
        manager.cardAsset = ca;
        manager.ReadCreatureFromAsset();
        manager.creatureLogic = creatureLogic;

        // add tag according to owner
        foreach (Transform t in creature.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString() + "Creature";
        // parent a new creature gameObject to table slots

        creature.transform.SetParent(slots.transform);
        // add a new creature to the list

        CreaturesOnTable.Insert(index, creature);

        // let this creature know about its position

        WhereIsTheCardOrCreature w = creature.GetComponent<WhereIsTheCardOrCreature>();
        w.Slot = index;
        if (owner == AreaPosition.Low)
            w.VisualState = VisualStates.LowTable;
        else
            w.VisualState = VisualStates.TopTable;


        IDHolder id = creature.AddComponent<IDHolder>();
        id.UniqueID = creatureLogic.UniqueCreatureID;

        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();

        Command.CommandExecutionComplete();
    }

    public int TablePosForNewCreature(float MouseX)
    {
        // if there are no creatures or if we are pointing to the right of all creatures with a mouse.
        // right - because the table slots are flipped and 0 is on the right side.    
        if (CreaturesOnTable.Count == 0 || MouseX > slots.Children[0].transform.position.x)
            return 0;
        else if (MouseX < slots.Children[CreaturesOnTable.Count - 1].transform.position.x) // cursor on the left relative to all creatures on the table
            return CreaturesOnTable.Count;
        for (int i = 0; i < CreaturesOnTable.Count; i++)
        {
            if (MouseX < slots.Children[i].transform.position.x && MouseX > slots.Children[i + 1].transform.position.x)
                return i + 1;
        }
        Debug.Log("Suspicious behavior. Reached end of TablePosForNewCreature method. Returning 0");
        return 0;
    }


    [ClientRpc]
    public void RpcRemoveCreatureWithID(int IDToRemove,Player p, bool sacrifice)
    {
        CreatureLogic creature = CreatureLogic.CreaturesCreatedThisGame[IDToRemove];
        GameObject creatureToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        
        //except from sacrificeing
        if (!sacrifice)
        {
            p.otherPlayer.TotalBones++;
        }

        //implement potential deathrattle later
        //if (effect != null)
        //    effect.WhenACreatureDies();

        p.graveYard.cards.Add(creature.ca);

        CreaturesOnTable.Remove(creatureToRemove);
        Destroy(creatureToRemove);


        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();
       
        Command.CommandExecutionComplete();
    }

    [Command(requiresAuthority = false)]
    public void RemoveCreatureWithID(int IDToRemove,Player p,bool sacrifice)
    {
        CreatureLogic creature = CreatureLogic.CreaturesCreatedThisGame[IDToRemove];
        p.table.CreaturesOnTable.Remove(creature);
        RpcRemoveCreatureWithID(IDToRemove, p, sacrifice);
    }

    /// <summary>
    /// Shifts the slots game object according to number of creatures.
    /// </summary>
    void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
    {
        float posX;
        if (CreaturesOnTable.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CreaturesOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    /// <summary>
    /// After a new creature is added or an old creature dies, this method
    /// shifts all the creatures and places the creatures on new slots.
    /// </summary>
    void PlaceCreaturesOnNewSlots()
    {
        foreach (GameObject g in CreaturesOnTable)
        {
            g.transform.DOLocalMoveX(slots.Children[CreaturesOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
        }
    }

}
