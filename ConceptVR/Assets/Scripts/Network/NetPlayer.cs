using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetPlayer : NetworkBehaviour {
    public static NetPlayer local;

    public int playerID;


	void Start () {
		if (isLocalPlayer)
        {
            local = this;
            playerID = GetComponent<NetworkIdentity>().netId;

            Debug.Log("I'm the local player, and my number is " + playerID);
        }
	}
	
	void Update () {
		
	}

    private void OnDestroy()
    {
        if (this == local)
            local = null;
    }
}
