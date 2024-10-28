using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VoidVisual : MonoBehaviour, IPointerClickHandler
{
    public AreaPosition owner;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            foreach (CardAsset card in GlobalSettings.Instance.Players[owner].voiid.cards)
            {
                Debug.Log(card);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
