using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkStarter : MonoBehaviour {
    public bool startHost;
	// Use this for initialization
	void Start () {
        if (startHost)
        {
            GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartHost();
            GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>().showGUI = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
