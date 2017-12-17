using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class NetworkManagerDH : NetworkManager
{
    public Text clientsInfoText;
    public ClientHUD clientHudScript;
    public ServerHUD serverHudScript;

    private int connectedClients = 0;

    [HideInInspector]
    public string serverPassword;
    //Server Side

    public GameObject players;
    public override void OnStartServer()
    {
        Debug.Log("OnStartServer");
        base.OnStartServer();
        RegisterServerHandles();

        serverPassword = serverHudScript.passwordText.text;
        connectedClients = 0;
        clientsInfoText.text = "Connected Clients : " + connectedClients;
    }

    //keeping track of Clients connecting.
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("OnServerConnect");
        base.OnServerConnect(conn);
        connectedClients += 1;
        clientsInfoText.text = "Connected Clients : " + connectedClients;

        //Sending password information to client.
        StringMessage msg = new StringMessage(serverPassword);
        NetworkServer.SendToClient(conn.connectionId, MsgType.Highest + 1, msg);
    }

    //keeping track of Clients disconnecting.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("OnServerDisconnect");
        base.OnServerDisconnect(conn);
        connectedClients -= 1;
        clientsInfoText.text = "Connected Clients : " + connectedClients;
    }

    public override void OnStopServer()
    {
        Debug.Log("OnStopServer");
        base.OnStopServer();
    }

    //Client Side
    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log("OnStartClient");
        base.OnStartClient(client);
        RegisterClientHandles();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("OnClientConnect");
        base.OnClientConnect(conn);
        clientHudScript.ConnectSuccses();
    }
    //when client recieves password information from the server.
    public void OnReceivePassword(NetworkMessage netMsg)
    {
        Debug.Log("OnReceivePassword");
        //read the server password.
        var msg = netMsg.ReadMessage<StringMessage>().value;
        //serverPassword = msg;
        if (msg != clientHudScript.passwordText.text)
            clientHudScript.DisConnect(true);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("OnClientDisconnect");
        base.OnClientDisconnect(conn);
        clientHudScript.DisConnect(false);
    }

    //Messages that need to be Registered on Server and Client Startup.
    void RegisterServerHandles()
    {
        Debug.Log("RegisterServerHandles");
        NetworkServer.RegisterHandler(MsgType.Highest + 1, OnReceivePassword);
    }

    void RegisterClientHandles()
    {
        Debug.Log("RegisterClientHandles");
        client.RegisterHandler(MsgType.Highest + 1, OnReceivePassword);
    }
}
