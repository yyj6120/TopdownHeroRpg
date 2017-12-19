using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class ServerHUD : MonoBehaviour {

    public GameObject stopServer, startServer, resetSettings, getIP, checking, clientsInfo;
    public Text serverInfoText, portPlaceholderText, paswPlaceholderText, clientsInfoText;
    public InputField portText, passwordText, maxConnText;

    private NetworkManagerDH manager;
    private bool noConnection, setText, checkIP;
    private string externalip="?", localIP="?";
    private int maximumConnections;

    void Start ()
    {
        Debug.Log("ServerHUD , Start");
        if (!manager)
            manager = NetworkManagerDH.Instance;

        if (PlayerPrefs.HasKey("nwPortS"))
        {
            manager.networkPort = Convert.ToInt32(PlayerPrefs.GetString("nwPortS"));
            portPlaceholderText.text = manager.networkPort.ToString();
        }
        if (PlayerPrefs.HasKey("IPAddressS"))
        {
            externalip = PlayerPrefs.GetString("IPAddressS");
            localIP = PlayerPrefs.GetString("LocalIP");
            getIP.GetComponentInChildren<Text>().text = "Server IP Address\nExternal :" + externalip + "\nLocal :" + localIP;
        }
        if (PlayerPrefs.HasKey("Password"))
        {
            passwordText.text = PlayerPrefs.GetString("Password");
            if (passwordText.text == "")
                paswPlaceholderText.text = "(not needed)";
        }
        if (PlayerPrefs.HasKey("MaxConnections"))
        {
            maxConnText.text = PlayerPrefs.GetString("MaxConnections");
        }

        clientsInfoText = clientsInfo.GetComponentInChildren<Text>();
        setText = true;       
    } 

	void Update () {
        
        noConnection = (manager.client == null || manager.client.connection == null ||
                     manager.client.connection.connectionId == -1);

        if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
        {
            if (noConnection)
            {
                stopServer.SetActive(false);
                clientsInfo.SetActive(false);

                if (setText)
                {
                    serverInfoText.color = Color.red;
                    serverInfoText.text = "Server Not Running !";
                    setText = false;
                }
            }
        }
        else
        {
            if (setText)
            {
                serverInfoText.color = new Color(0.2f, 0.6f, 0.2f, 1f);
                string pw="";               
                if (passwordText.text == "")               
                    pw = "(no password)";                
                else pw = passwordText.text;

                string maxConn = "";
                if (maxConnText.text == "")
                    maxConn = "8";
                else maxConn = maxConnText.text;

                serverInfoText.text = "Server Is Running !\n" +"\nIP Address\nExternal : " +externalip+"\nLocal : "+localIP+"\n\nServer Port : " + manager.networkPort+"\nPassword : "+pw+"\nMax Connections : "+ maxConn;
                setText = false;
            }
        }
    }

    public void StopHostCustom()
    {
        Debug.Log("StopHostCustom");
        startServer.SetActive(true);
        portText.transform.parent.gameObject.SetActive(true);
        resetSettings.SetActive(true);
        getIP.SetActive(true);
        setText = true;
        manager.StopHost();
    }

    public void StartServerCustom()
    {
        Debug.Log("StartServerCustom");

        if (portText.text == "")
        {
            if (PlayerPrefs.HasKey("nwPortS"))
            {
                manager.networkPort = Convert.ToInt32(PlayerPrefs.GetString("nwPortS"));
            }
            else
            {
                manager.networkPort = 7777;
                portPlaceholderText.text = manager.networkPort.ToString()+"(Default)";
            }
        }
        else
        {
            PlayerPrefs.SetString("nwPortS", portText.text);      
            manager.networkPort = Convert.ToInt32(portText.text);
            portPlaceholderText.text = manager.networkPort.ToString();
        }

        PlayerPrefs.SetString("Password", passwordText.text);
        PlayerPrefs.SetString("MaxConnections", maxConnText.text.ToString());
        
        resetSettings.SetActive(false);
        portText.transform.parent.gameObject.SetActive(false);
        getIP.SetActive(false);
        startServer.SetActive(false);
        stopServer.SetActive(true);
        clientsInfo.SetActive(true);
        setText = true;

        if (maxConnText.text != "")
        {
            maximumConnections = Convert.ToInt32(maxConnText.text);
        }
        else maximumConnections = 8;
        manager.maxConnections = maximumConnections;

        manager.StartServer();
    }

    public void ResetToDefault()
    {
        Debug.Log("ResetToDefault");
        PlayerPrefs.DeleteKey("IPAddressS");
        getIP.GetComponentInChildren<Text>().text = "Find Server IP Address.";
        externalip = "?";
        PlayerPrefs.DeleteKey("nwPortS");
        portPlaceholderText.text = "7777(Default)";
        portText.text = "";
        PlayerPrefs.DeleteKey("LocalIP");
        localIP = "?";
        PlayerPrefs.DeleteKey("Password");
        paswPlaceholderText.text = "(not needed)";
        passwordText.text = "";
        PlayerPrefs.DeleteKey("MaxConnections");
        maxConnText.text = "";
    }

    public void GetIP()
    {
        Debug.Log("GetIP");
        getIP.GetComponentInChildren<Text>().text = "If this takes too long\nClick again.";
        StartCoroutine(GetPublicIP());
        checking.SetActive(true);
    }

    IEnumerator GetPublicIP()
    {
        Debug.Log("GetPublicIP");
        WWW www = new WWW("http://checkip.dyndns.org");
        yield return www;
        if (www.error==null)
        {
            string response = www.text;
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            externalip = a4;

            localIP = Network.player.ipAddress;

            getIP.GetComponentInChildren<Text>().text = "Server IP Address\nExternal :" + externalip+"\nLocal :"+localIP;

            PlayerPrefs.SetString("IPAddressS", externalip);
            PlayerPrefs.SetString("LocalIP", localIP);
            checking.SetActive(false);
        }
        else
        {
            getIP.GetComponentInChildren<Text>().text = "Someting went wrong\nPlease try again";
            checking.SetActive(false);
        }
    }
}
