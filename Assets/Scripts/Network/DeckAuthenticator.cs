using Mirror;
using System.Collections.Generic;

public class DeckAuthenticator : NetworkAuthenticator
{
    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler<AuthenticationRequestMessage>(OnAuthenticationMessageReceived, false);
    }

    public override void OnClientAuthenticate()
    {
        AuthenticationRequestMessage authRequest = new AuthenticationRequestMessage
        {
            deckName = StaticVariables.deckName,
            charAssetName = StaticVariables.charAssetName,
            cardNames = StaticVariables.cardNames
        };
        NetworkClient.Send(authRequest);
        ClientAccept();
    }

    private void OnAuthenticationMessageReceived(NetworkConnectionToClient conn, AuthenticationRequestMessage authRequest)
    {
        conn.authenticationData = new DeckData
        {
            deckName = authRequest.deckName,
            charAssetName = authRequest.charAssetName,
            cardNames = authRequest.cardNames
        };

        ServerAccept(conn);
    }
}

//Authentication message sent by the client
public struct AuthenticationRequestMessage : NetworkMessage
{
    public string deckName;
    public string charAssetName;
    public List<string> cardNames;
}

//Data structure to store deck information on the server
public class DeckData
{
    public string deckName;
    public string charAssetName;
    public List<string> cardNames;
}
