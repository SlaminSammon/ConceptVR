using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkStarter : MonoBehaviour {
    public bool startHost;
    private NetworkManager netManager;
    bool connecting = false;
	// Use this for initialization
	void Start () {
        netManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        if (startHost)
        {
            netManager.StartHost();
            GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>().showGUI = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!netManager.isNetworkActive && !connecting && startHost)
        {
            Debug.Log("Starting new Host");
            netManager.networkAddress = "localhost";
            netManager.StartHost();
        }
        if(netManager.isNetworkActive && connecting && Network.isClient)
        {
            connecting = false;
        }
    }
    public void connectToHost(string netAddress, int netPort)
    {
        netManager.StopHost();
        netManager.networkAddress = netAddress;
        netManager.networkPort = netPort;
        netManager.StartClient();
        connecting = true;
    }
}
