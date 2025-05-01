using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsResetter : MonoBehaviour {

    public bool DeleteAllFromPlayerPrefs = false;
	void Awake () 
    {
        if (DeleteAllFromPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
        }
	}	
}
