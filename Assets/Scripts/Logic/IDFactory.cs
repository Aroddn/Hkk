using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates IDs to be used to identify cards

public class IDFactory
{
    private static int Count;

    public static int GetUniqueID()
    {
        Count++;
        return Count;
    }

    public static void ResetIDs()
    {
        Count = 0;
    }
}
