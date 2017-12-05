using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class NetworkManagerDH : NetworkManager
{

    public List<NetworkPlayer> connectedPlayers
    {
        get;
        private set;
    }

    private SceneChangeMode sceneChangeMode;

    [SerializeField]
    protected NetworkPlayer networkPlayerPrefab;

    public override void OnStartHost()
    {
        Debug.Log("OnStartHost");
        base.OnStartHost();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("OnServerConnect");
        base.OnServerConnect(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("OnClientConnect");
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(0);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("OnServerReady");
        base.OnServerReady(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        // Intentionally not calling base here - we want to control the spawning of prefabs
        Debug.Log("OnServerAddPlayer");
        NetworkPlayer newPlayer = Instantiate(networkPlayerPrefab);
        DontDestroyOnLoad(newPlayer);
        NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject, playerControllerId);
    }

    public void RegisterNetworkPlayer(NetworkPlayer newPlayer)
    {
        connectedPlayers.Add(newPlayer);
        sceneChangeMode = SceneChangeMode.Game;
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("OnServerSceneChanged");
        base.OnServerSceneChanged(sceneName);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("OnClientSceneChanged");
        base.OnClientSceneChanged(conn);

        NetworkPlayer np = connectedPlayers[0];
        np.OnEnterGameScene();
    }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            connectedPlayers = new List<NetworkPlayer>();
        }
    }

    protected virtual void Update()
    {
        if (sceneChangeMode == SceneChangeMode.None)
        {

        }
        else
        {
            ServerChangeScene("InGameScene");
        }
        sceneChangeMode = SceneChangeMode.None;
    }

    public static NetworkManagerDH Instance
    {
        get;
        protected set;
    }
}
