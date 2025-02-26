using Mirror;
using System.Collections.Generic;

public class DeckAuthenticator : NetworkAuthenticator
{
    // Called when the server starts; register the handler
    public override void OnStartServer()
    {
        // Register the handler globally for AuthenticationRequestMessage
        NetworkServer.RegisterHandler<AuthenticationRequestMessage>(OnAuthenticationMessageReceived, false); // 'false' allows pre-authentication
    }

    // Called when the client starts authentication
    public override void OnClientAuthenticate()
    {
        // Create and send the AuthenticationRequestMessage
        AuthenticationRequestMessage authRequest = new AuthenticationRequestMessage
        {
            deckName = StaticVariables.deckName,
            cardNames = StaticVariables.cardNames
        };

        // Send the message to the server
        NetworkClient.Send(authRequest);

        // Notify that the client has completed authentication
        ClientAccept();
    }

    // Handles the AuthenticationRequestMessage during the pre-authentication phase
    private void OnAuthenticationMessageReceived(NetworkConnectionToClient conn, AuthenticationRequestMessage authRequest)
    {
        // Store the data in the connection's authenticationData
        conn.authenticationData = new DeckData
        {
            deckName = authRequest.deckName,
            cardNames = authRequest.cardNames
        };

        // Debug logs to verify the data
        //UnityEngine.Debug.Log($"Deck Name: {authRequest.deckName}");
        //UnityEngine.Debug.Log($"Card Names: {string.Join(", ", authRequest.cardNames)}");

        // Complete the server-side authentication
        ServerAccept(conn);
    }
}

// Authentication message sent by the client
public struct AuthenticationRequestMessage : NetworkMessage
{
    public string deckName;
    public List<string> cardNames;
}

// Data structure to store deck information on the server
public class DeckData
{
    public string deckName;
    public List<string> cardNames;
}
