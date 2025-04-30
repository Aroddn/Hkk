using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using System.Linq;
using Unity.VisualScripting;

public class TableVisual : NetworkBehaviour
{
    public AreaPosition owner;
    public SameDistanceChildren slots;
    public List<GameObject> CreaturesOnTable = new List<GameObject>();
    private bool cursorOverThisTable = false;
    private BoxCollider col;

    public static bool CursorOverSomeTable
    {
        get
        {
            TableVisual[] bothTables = GameObject.FindObjectsOfType<TableVisual>();
            return (bothTables[0].CursorOverThisTable || bothTables[1].CursorOverThisTable);
        }
    }

    public bool CursorOverThisTable
    {
        get{ return cursorOverThisTable; }
    }

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    void Update()
    {
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

        OneCreatureManager manager = creature.GetComponent<OneCreatureManager>();
        manager.cardAsset = ca;
        manager.ReadCreatureFromAsset();
        manager.creatureLogic = creatureLogic;


        foreach (Transform t in creature.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString() + "Creature";

        creature.transform.SetParent(slots.transform);

        CreaturesOnTable.Insert(index, creature);


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
        if (CreaturesOnTable.Count == 0 || MouseX > slots.Children[0].transform.position.x)
            return 0;
        else if (MouseX < slots.Children[CreaturesOnTable.Count - 1].transform.position.x)
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

        //except from sacrificing
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

    void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
    {
        float posX;
        if (CreaturesOnTable.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CreaturesOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    void PlaceCreaturesOnNewSlots()
    {
        foreach (GameObject g in CreaturesOnTable)
        {
            g.transform.DOLocalMoveX(slots.Children[CreaturesOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
        }
    }

}
