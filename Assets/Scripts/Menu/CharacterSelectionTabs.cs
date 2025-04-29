using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionTabs : MonoBehaviour 
{
    public List<CharacterFilterTab> Tabs = new List<CharacterFilterTab>();
    public CharacterFilterTab ClassTab;
    public CharacterFilterTab NeutralTabWhenCollectionBrowsing;
    private int currentIndex = 0;

    public void SelectTab(CharacterFilterTab tab, bool instant)
    {
        int newIndex = Tabs.IndexOf(tab);

        if (newIndex == currentIndex)
            return;

        currentIndex = newIndex;

        foreach (CharacterFilterTab t in Tabs)
        {
            if (t != tab)
                t.Deselect(instant);
        }
        tab.Select(instant);
        DeckBuildingScreen.Instance.CollectionBrowserScript.Asset = tab.Asset;
        DeckBuildingScreen.Instance.CollectionBrowserScript.Set = tab.setName;
        DeckBuildingScreen.Instance.CollectionBrowserScript.IncludeAllCharacters = tab.showAllCharacters;
    }

    public void SetClassOnClassTab(CharacterAsset asset)
    {
        ClassTab.Asset = asset;
        ClassTab.GetComponentInChildren<Text>().text = "Neutral";
    }
}
