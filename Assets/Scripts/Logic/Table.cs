using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

//holds the creaturelogic for cards on the table for the given player

public class Table : NetworkBehaviour
{
    public List<CreatureLogic> CreaturesOnTable = new List<CreatureLogic>();
    public void PlaceCreatureAt(int index, CreatureLogic creature)
    {
        CreaturesOnTable.Insert(index, creature);
    }
        
}
