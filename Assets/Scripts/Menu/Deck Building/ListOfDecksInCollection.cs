using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ListOfDecksInCollection : MonoBehaviour {

    public Transform Content;
    public GameObject DeckInListPrefab;
    public GameObject NewDeckButtonPrefab;

    public void UpdateList()
    {
        foreach (Transform t in Content)
        {
            if (t != Content)
            {
                Destroy(t.gameObject);
            }
        }
        foreach (DeckInfo info in DecksStorage.Instance.AllDecks)
        {
            GameObject g = Instantiate(DeckInListPrefab, Content);
            g.transform.localScale = Vector3.one;
            DeckInScrollList deckInScrollListComponent = g.GetComponent<DeckInScrollList>();
            deckInScrollListComponent.ApplyInfo(info);
        }

        if (DecksStorage.Instance.AllDecks.Count < 9)
        {     
            GameObject g = Instantiate(NewDeckButtonPrefab, Content);
            g.transform.localScale = Vector3.one;
        }
    }
        
}
