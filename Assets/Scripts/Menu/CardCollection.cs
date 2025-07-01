using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardCollection : MonoBehaviour 
{
    public int DefaultNumberOfBasicCards = 4;

    public static CardCollection Instance;
    private Dictionary<string, CardAsset > AllCardsDictionary = new Dictionary<string, CardAsset>();

    private Dictionary<string, CharacterAsset> AllCharacterAssetDictionary = new Dictionary<string, CharacterAsset>();

    public Dictionary<CardAsset, int> QuantityOfEachCard = new Dictionary<CardAsset, int>();

    private CardAsset[] allCardsArray;
    private CharacterAsset[] allCharacterArray;

    void Awake()
    {
        Instance = this;

        allCardsArray = Resources.LoadAll<CardAsset>("");
        allCharacterArray = Resources.LoadAll<CharacterAsset>("");
        
        foreach (CardAsset ca in allCardsArray)
        {
            if (!AllCardsDictionary.ContainsKey(ca.name))
                AllCardsDictionary.Add(ca.name, ca);
        }

        foreach (CharacterAsset ca in allCharacterArray)
        {
            if (!AllCharacterAssetDictionary.ContainsKey(ca.ClassName))
                AllCharacterAssetDictionary.Add(ca.ClassName, ca);
        }

        LoadQuantityOfCardsFromPlayerPrefs();
    }

    private void LoadQuantityOfCardsFromPlayerPrefs()
    {
        foreach (CardAsset ca in allCardsArray)
        {
            QuantityOfEachCard.Add(ca, DefaultNumberOfBasicCards);
            //if (ca.Rarity == RarityOptions.ALL)
            //    QuantityOfEachCard.Add(ca, DefaultNumberOfBasicCards);            
            //else if (PlayerPrefs.HasKey("NumberOf" + ca.name))
            //    QuantityOfEachCard.Add(ca, PlayerPrefs.GetInt("NumberOf" + ca.name));
            //else
            //    QuantityOfEachCard.Add(ca, 0);
        }
    }

    private void SaveQuantityOfCardsIntoPlayerPrefs()
    {
        foreach (CardAsset ca in allCardsArray)
        {
            PlayerPrefs.SetInt("NumberOf" + ca.name, DefaultNumberOfBasicCards);
            //if (ca.Rarity == RarityOptions.ALL)
            //    PlayerPrefs.SetInt("NumberOf" + ca.name, DefaultNumberOfBasicCards);
            //else
            //    PlayerPrefs.SetInt("NumberOf" + ca.name, QuantityOfEachCard[ca]);
        }
    }

    void OnApplicationQuit()
    {
        SaveQuantityOfCardsIntoPlayerPrefs();
    }

    public CardAsset GetCardAssetByName(string name)
    {        
        if (AllCardsDictionary.ContainsKey(name))
            return AllCardsDictionary[name];
        else 
            return null;
    }

    public CharacterAsset GetCharacterAssetByName(string name)
    {
        if (AllCharacterAssetDictionary.ContainsKey(name))
            return AllCharacterAssetDictionary[name];
        else 
            return null;
    }

    public List<CardAsset> GetCardsOfCharacter(CharacterAsset asset)
    {   
        return GetCards(true, true, false, RarityOptions.ALL, asset);
    }

    public List<CardAsset> GetCardsWithRarity(RarityOptions rarity)
    {
        return GetCards(true, false, true, rarity);
    }

    public List<CardAsset> GetCards(bool showingCardsPlayerDoesNotOwn = false, bool includeAllRarities = true, bool includeAllCharacters = true, RarityOptions rarity = RarityOptions.ALL,
                CharacterAsset asset = null, Set set = Set.All,string keyword = "", int manaCost = -1, bool includeTokenCards = false)
    {
        var cards = from card in allCardsArray select card;

        if (!showingCardsPlayerDoesNotOwn)
            cards = cards.Where(card => QuantityOfEachCard[card] > 0);

        if (!includeTokenCards)
            cards = cards.Where(card => card.TokenCard == false);

        if (!includeAllRarities)
            cards = cards.Where(card => card.Rarity == rarity);

        if(set != Set.All)
            cards = cards.Where(card => card.setName == set);

        if (keyword != null && keyword != "")
            cards = cards.Where(card => (card.name.ToLower().Contains(keyword.ToLower()) || 
                (card.Tags.ToLower().Contains(keyword.ToLower()) && !keyword.ToLower().Contains(" "))));

        if (manaCost == 7)
            cards = cards.Where(card => card.ManaCost >= 7);
        else if (manaCost != -1)
            cards = cards.Where(card => card.ManaCost == manaCost);                
        
        var returnList = cards.ToList<CardAsset>();
        returnList.Sort();

        return returnList;
    }
}
