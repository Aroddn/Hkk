using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsResetter : MonoBehaviour {

    // use this once to reset all the values to default.
    public bool DeleteAllFromPlayerPrefs = false;
	void Awake () 
    {
        if (DeleteAllFromPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
        }
	}	
}
