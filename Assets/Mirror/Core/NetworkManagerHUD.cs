using UnityEngine;

namespace Mirror
{
    /// <summary>Shows NetworkManager controls in a GUI at runtime.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Manager HUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class NetworkManagerHUD : MonoBehaviour
    {
        NetworkManager manager;

        public int offsetX;
        public int offsetY;

        string username = "";
        public bool showGUI = true;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();

            // Set last username used (if any) in the username's input field
            if (PlayerPrefs.GetString("Name") != null) username = PlayerPrefs.GetString("Name");
        }

        void OnGUI()
        {

            if (!showGUI)
                return;

            // If this width is changed, also change offsetX in GUIConsole::OnGUI

            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));

            if (!NetworkClient.isConnected && !NetworkServer.active)
                StartButtons();
            else
                StatusLabels();

            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    // client ready
                    NetworkClient.Ready();
                    if (NetworkClient.localPlayer == null)
                        NetworkClient.AddPlayer();
                }
            }

            StopButtons();

            GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("Host (Server + Client)"))
                    {
                        manager.StartHost();

                        // Save the player's username
                        PlayerPrefs.SetString("Name", username);

                        // Hide GUI
                        showGUI = false;
                    }
                }

                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client"))
                {
                    manager.StartClient();

                    // Save the player's username
                    PlayerPrefs.SetString("Name", username);

                    // Hide GUI
                    showGUI = false;
                }
                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                GUILayout.EndHorizontal();

                // Username field
                username = GUILayout.TextField(username);


                // Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                else
                {
                    if (GUILayout.Button("Server Only")) manager.StartServer();
                }
            }
            else
            {
                // Connecting
                GUILayout.Label("Connecting to " + manager.networkAddress + "..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    manager.StopClient();
                }
            }
        }

        void StatusLabels()
        {
            if (NetworkServer.active)
            {
                GUILayout.Label("Server: active. Transport: " + Transport.active);
            }
            if (NetworkClient.isConnected)
            {
                GUILayout.Label("Client: address=" + manager.networkAddress);
            }
        }

        void StopButtons()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                GUILayout.BeginHorizontal();
#if UNITY_WEBGL
                if (GUILayout.Button("Stop Single Player"))
                    manager.StopHost();
#else
                // stop host if host mode
                if (GUILayout.Button("Stop Host"))
                    manager.StopHost();

                // stop client if host mode, leaving server up
                if (GUILayout.Button("Stop Client"))
                    manager.StopClient();
#endif
                GUILayout.EndHorizontal();
            }
            else if (NetworkClient.isConnected)
            {
                // stop client if client-only
                if (GUILayout.Button("Stop Client"))
                    manager.StopClient();
            }
            else if (NetworkServer.active)
            {
                // stop server if server-only
                if (GUILayout.Button("Stop Server"))
                    manager.StopServer();
            }
        }
    }
}
