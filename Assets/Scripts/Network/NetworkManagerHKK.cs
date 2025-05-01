using UnityEngine;
using Mirror;

[AddComponentMenu("Network Manager HKK")]
public class NetworkManagerHKK : NetworkManager
{
    public bool SpawnAsCharacter = true;

    public static new NetworkManagerHKK singleton => (NetworkManagerHKK)NetworkManager.singleton;


    public override void Awake()
    {
        base.Awake();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

        if (conn.identity != null)
        {
            Debug.LogWarning("Player object already exists for this connection.");
            return;
        }

        //Spawn the player prefab
        GameObject playerObject = Instantiate(playerPrefab);

        //Retrieve deck data from authenticationData
        if (conn.authenticationData is DeckData deckData)
        {
            //Apply the deck data to the player
           
            Player player = playerObject.GetComponent<Player>();
            player.deckName = deckData.deckName;
            deckData.cardNames.Shuffle();
            player.cardNames = deckData.cardNames;
            player.charAssetName = deckData.charAssetName;
            player.charAsset = CardCollection.Instance.GetCharacterAssetByName(deckData.charAssetName);

            foreach (string name in deckData.cardNames)
            {
                {
                    player.deck.cards.Add(CardCollection.Instance.GetCardAssetByName(name));
                }
            }
        }
        //Add the player to the server
        NetworkServer.AddPlayerForConnection(conn, playerObject);
    }
}
