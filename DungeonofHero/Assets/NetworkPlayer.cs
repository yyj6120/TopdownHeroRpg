using System;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkPlayer : NetworkBehaviour
{
    NetworkManagerDH netmanager;
    public GameObject player;
    [Client]
    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");

        DontDestroyOnLoad(this);
        netmanager = NetworkManagerDH.Instance;
        base.OnStartClient();
        netmanager.RegisterNetworkPlayer(this);
    }

    [Client]
    public override void OnStartLocalPlayer()
    {
        Debug.Log("OnStartLocalPlayer");
    }

    [Client]
    public void OnEnterGameScene()
    {
        if (hasAuthority)
        {
            GameObject localplayer = Instantiate(player);
            NetworkServer.SpawnWithClientAuthority(localplayer, connectionToClient);
        }
    }

}

