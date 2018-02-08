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
            playerID = playerControllerId;
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
