using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GraveYardVisual : MonoBehaviour, IPointerClickHandler
{
    public AreaPosition owner;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            foreach (CardAsset card in GlobalSettings.Instance.Players[owner].graveYard.cards)
            {
                Debug.Log(card);
            }
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
