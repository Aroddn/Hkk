using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class ManaPoolVisual : MonoBehaviour {

    public int currentMana;
    public int maxMana = 20;
    
    private int totalCrystals;
    public int TotalCrystals
    {
        get{ return totalCrystals; }

        set
        {
            totalCrystals = value;
            ProgressText.text = string.Format("{0}/{1}", availableCrystals.ToString(), totalCrystals.ToString());
        }
    }

    private int availableCrystals = 20;
    public int AvailableCrystals
    {
        get{ return availableCrystals; }

        set
        {
            if(value > totalCrystals)
                availableCrystals = totalCrystals;
            else if (value < 0)
                availableCrystals = 0;
            else
                availableCrystals = value;
            ProgressText.text = string.Format("{0}/{1}", availableCrystals.ToString(), totalCrystals.ToString());
        }
    }

    public TMP_Text ProgressText;

    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            AvailableCrystals = currentMana;
            TotalCrystals = maxMana;
        }
    }

}
