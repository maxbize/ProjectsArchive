using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour {

    private string gameTypeName = "Sniper_Wars_a8w4d";
    private string gameName = "Public Beta Server";



	// Use this for initialization
	void Start () {
        //MasterServer.RequestHostList(gameTypeName);

        FindObjectOfType<TeamManager>().spawnPlayer(); // Bypassing all network logic and jumping straight to spawn
    }

    // Update is called once per frame
    void Update () {
	
	}

    /*
    void startServer() {
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(gameTypeName, gameName);
    }

    void OnMasterServerEvent(MasterServerEvent mse) {
        // Server created
        if (mse == MasterServerEvent.RegistrationSucceeded) {
            Debug.Log("Server Registration Succeeded!");
        }
        // Server exists - get it!
        else if (mse == MasterServerEvent.RegistrationFailedGameName) {
            Debug.Log("Server exists, attempting to get it!");
            MasterServer.RequestHostList(gameTypeName);
        }
        // Found the server, connect to it!
        else if (mse == MasterServerEvent.HostListReceived) {
            if (MasterServer.PollHostList().Length == 0) {
                Debug.Log("No servers found. Starting one");
                startServer();
            }
            else {
                Debug.Log("Found an existing server. Attempting to connect...");
                Network.Connect(MasterServer.PollHostList()[0]);
            }
        }
        else {
            Debug.Log("Unable to connect to server! Event: " + mse);
        }
    }
    */

    public IEnumerator OnConnectedToServer() {
        Debug.Log("Connected to server!!");
        yield return new WaitForFixedUpdate(); // We have to wait for the player list to populate
        FindObjectOfType<TeamManager>().spawnPlayer();
    }

    public void OnServerInitialized() {
        Debug.Log("Server has been created");
        FindObjectOfType<TeamManager>().spawnPlayer();
    }

    /*
    void OnDisconnectedFromServer(NetworkDisconnection info) {
        Debug.Log("Disconnected from server: " + info);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }
    */
}
