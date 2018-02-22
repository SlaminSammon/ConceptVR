using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkStarter : MonoBehaviour {
	// Use this for initialization
	void Start () {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartHost();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
