using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterFilterTab : MonoBehaviour {

    public CharacterAsset Asset;
    public Set setName;

    public bool showAllCharacters = false;
    private CharacterSelectionTabs TabsScript;
    private float selectionTransitionTime = 0.5f;
    private Vector3 initialScale = Vector3.one;
    private float scaleMultiplier = 1.2f;

    public void TabButtonHandler()
    {
        DeckBuildingScreen.Instance.TabsScript.SelectTab(this, false);
    }

    public void Select(bool instant = false)
    {
        if (instant)
            transform.localScale = initialScale * scaleMultiplier;
        else
            transform.DOScale(initialScale.x * scaleMultiplier, selectionTransitionTime);
    }

    public void Deselect(bool instant = false)
    {
        if (instant)
            transform.localScale = initialScale;
        else
            transform.DOScale(initialScale, selectionTransitionTime);
    }
}
