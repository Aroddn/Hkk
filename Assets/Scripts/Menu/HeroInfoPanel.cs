using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class HeroInfoPanel : MonoBehaviour {

    public PlayerPortraitVisual portrait;
    public Button PlayButton;
    public Button BuildDeckButton;
    public PortraitMenu selectedPortrait{ get; set;}
    public DeckIcon selectedDeck{ get; set;}

    void Awake()
    {
        OnOpen();
    }
        
    public void OnOpen()
    {
        SelectCharacter(null);
        SelectDeck(null);
    }

    public void SelectCharacter(PortraitMenu menuPortrait)
    {
        if (menuPortrait == null || selectedPortrait == menuPortrait)
        {
            portrait.gameObject.SetActive(false);
            selectedPortrait = null;
            if (BuildDeckButton!=null)
                BuildDeckButton.interactable = false;
        }
        else
        {            
            portrait.charAsset = menuPortrait.asset;
            portrait.ApplyLookFromAsset();
            portrait.gameObject.SetActive(true);
            selectedPortrait = menuPortrait;
            if (BuildDeckButton!=null)
                BuildDeckButton.interactable = true;
        }
    }

    public void SelectDeck(DeckIcon deck)
    {
        if (deck == null || selectedDeck == deck || !deck.DeckInformation.IsComplete())
        {
            portrait.gameObject.SetActive(false);
            selectedDeck = null;
            if (PlayButton!=null)
                PlayButton.interactable = false;
        }
        else
        {           
            portrait.charAsset = deck.DeckInformation.Character;
            portrait.ApplyLookFromAsset();
            portrait.gameObject.SetActive(true);
            selectedDeck = deck;
            BattleStartInfo.SelectedDeck = selectedDeck.DeckInformation;
            StaticVariables.deckName = BattleStartInfo.SelectedDeck.DeckName;
            StaticVariables.charAssetName = BattleStartInfo.SelectedDeck.Character.ClassName;

            StaticVariables.cardNames.Clear();
            foreach (CardAsset card in BattleStartInfo.SelectedDeck.Cards)
            {
                StaticVariables.cardNames.Add(card.cardName);
            }
            if (PlayButton!=null)
                PlayButton.interactable = true;
        }
    }

    public void GoToDeckbuilding()
    {
        if (selectedPortrait == null)
            return;

        DeckBuildingScreen.Instance.BuildADeckFor(selectedPortrait.asset);
    }
        
}
