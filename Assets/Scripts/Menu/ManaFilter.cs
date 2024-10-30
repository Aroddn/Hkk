using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaFilter : MonoBehaviour {

    public Image[] Crystals;
    public Color32 HighlightedColor = new Color32(255, 255, 255, 255);
    public Color32 UnactiveColor = new Color32(128, 128, 128, 255);

    private int currentIndex = -1;

    void Start()
    {
        RemoveAllFilters();
        currentIndex = -1;
        DeckBuildingScreen.Instance.CollectionBrowserScript.ManaCost = currentIndex;
    }

    public void PressOnCrystal(int index)
    {
        RemoveAllFilters();
        if (index != currentIndex)
        {
            currentIndex = index;
            Crystals[index].color = HighlightedColor;
        }
        else
        {
            currentIndex = -1;
        }

        DeckBuildingScreen.Instance.CollectionBrowserScript.ManaCost = currentIndex;
    }

    public void RemoveAllFilters()
    {
        foreach (Image i in Crystals)
            i.color = UnactiveColor;
    }
}
