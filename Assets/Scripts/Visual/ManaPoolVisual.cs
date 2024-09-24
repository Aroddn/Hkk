using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class ManaPoolVisual : MonoBehaviour {

    public int start;
    public int max = 20;
    
    private int maxMana = 20;
    private int currentMana;
    public int MaxMana
    {
        get{ return maxMana; }

        set
        {
            maxMana = value;
            ProgressText.text = string.Format("{0}/{1}", currentMana.ToString(), maxMana.ToString());
        }
    }

    public int CurrentMana
    {
        get{ return currentMana; }

        set
        {
            if(value > maxMana)
                currentMana = maxMana;
            else if (value < 0)
                currentMana = 0;
            else
                currentMana = value;
            ProgressText.text = string.Format("{0}/{1}", currentMana.ToString(), maxMana.ToString());
        }
    }

    public TMP_Text ProgressText;

    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            CurrentMana = start;
            MaxMana = max;
        }
    }

}
