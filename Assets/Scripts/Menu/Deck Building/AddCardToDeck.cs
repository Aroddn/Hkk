using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AddCardToDeck : MonoBehaviour {

    public Text QuantityText;
    private float InitialScale;
    private float scaleFactor = 1.1f;
    private CardAsset cardAsset;

    void Awake()
    {
        InitialScale = transform.localScale.x;
    }

    public void SetCardAsset(CardAsset asset) { cardAsset = asset; } 

    void OnMouseDown()
    {
        CardAsset asset = GetComponent<OneCardManager>().cardAsset;
        if (asset == null)
            return;

        if (CardCollection.Instance.QuantityOfEachCard[cardAsset] - DeckBuildingScreen.Instance.BuilderScript.NumberOfThisCardInDeck(cardAsset) > 0)
        {
            DeckBuildingScreen.Instance.BuilderScript.AddCard(asset);
            UpdateQuantity();
        }
        else
        {
            // say that you do not have enough cards
        }
    }

    void OnMouseEnter()
    {        
        transform.DOScale(InitialScale*scaleFactor, 0.5f);
    }

    void OnMouseExit()
    {
        transform.DOScale(InitialScale, 0.5f);
    }

    void Update () 
    {
    }


    public void UpdateQuantity()
    {
        int quantity = CardCollection.Instance.QuantityOfEachCard[cardAsset];

        if (DeckBuildingScreen.Instance.BuilderScript.InDeckBuildingMode && DeckBuildingScreen.Instance.ShowReducedQuantitiesInDeckBuilding)
            quantity -= DeckBuildingScreen.Instance.BuilderScript.NumberOfThisCardInDeck(cardAsset);
        
        QuantityText.text = "X" + quantity.ToString();

    }
}
