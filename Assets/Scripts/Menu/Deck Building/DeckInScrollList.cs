using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DeckInScrollList : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Image AvatarImage;
    public Text NameText;
    public GameObject DeleteDeckButton;
    public DeckInfo savedDeckInfo;

    public void Awake()
    {
        DeleteDeckButton.SetActive(false);
    }

    public void EditThisDeck()
    {
        ConfirmationDialog.Hide();
        DeckBuildingScreen.Instance.HideScreen();
        DeckBuildingScreen.Instance.BuilderScript.BuildADeckFor(savedDeckInfo.Character);
        DeckBuildingScreen.Instance.BuilderScript.DeckName.text = savedDeckInfo.DeckName;
        foreach (CardAsset asset in savedDeckInfo.Cards)
            DeckBuildingScreen.Instance.BuilderScript.AddCard(asset);

        DecksStorage.Instance.AllDecks.Remove(savedDeckInfo);

        DeckBuildingScreen.Instance.TabsScript.SetClassOnClassTab(savedDeckInfo.Character);
        DeckBuildingScreen.Instance.CollectionBrowserScript.ShowCollectionForDeckBuilding(savedDeckInfo.Character);
        // TODO: save the index of this deck not to make it shift to the end of the list of decks and add it to the same place.

        DeckBuildingScreen.Instance.ShowScreenForDeckBuilding();
    }

    public void DeleteThisDeck()
    {
        ConfirmationDialog.Show("Are you sure you want to delete this deck?", () =>
        {
            DecksStorage.Instance.AllDecks.Remove(savedDeckInfo);
            DecksStorage.Instance.SaveDecksIntoPlayerPrefs();
            Destroy(gameObject);
        });
    }

    public void ApplyInfo (DeckInfo deckInfo)
    {
        AvatarImage.sprite = deckInfo.Character.AvatarImage;
        NameText.text = deckInfo.DeckName;
        savedDeckInfo = deckInfo;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // show delete deck button
        DeleteDeckButton.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        // hide delete deck button
        DeleteDeckButton.SetActive(false);
    }
}
