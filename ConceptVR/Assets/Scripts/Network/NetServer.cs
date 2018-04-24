using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetServer : NetworkManager {
    private NetworkStarter netStarter;
    public void Start() { 
        netStarter = GameObject.Find("NetworkStarter").GetComponent<NetworkStarter>();
    }
    public override void OnStartHost()
    {
        Debug.Log("Hosting match at: " + this.networkAddress + ":" + this.networkPort);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkServer.DestroyPlayersForConnection(conn);
        if (conn.lastError != NetworkError.Ok)
        {
            if (LogFilter.logError)
            {
                Debug.LogError("ServerDisconnected due to error: " + conn.lastError);
            }
        }
    }
}
