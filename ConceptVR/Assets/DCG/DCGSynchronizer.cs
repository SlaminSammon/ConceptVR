using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DCGSynchronizer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    [Command]
    public void CmdAddPoint()
    {

    }

    [ClientRpc]
    public void RpcAddPoint()
    {

    }
}
