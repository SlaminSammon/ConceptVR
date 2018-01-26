using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DCGSynchronizer : NetworkBehaviour {
    public struct ExpectedElement
    {
        public int id;
        public List<int> requirers;
    }

    public class ElementPacket
    {
        public int id;
        public int[] requirements;
        public ElementType type;
    }

    public class PointPacket : ElementPacket
    {
        public Vector3 position;
    }


    Dictionary<int, ExpectedElement> expectations;
    List<ElementPacket> waiting;


	void Start () {
        expectations = new Dictionary<int, ExpectedElement>();
        waiting = new List<ElementPacket>();
	}

    void Receive(ElementPacket e)
    {
        bool hasreqs = true;
        foreach (int r in e.requirements)
        {
            if (!DCGBase.all.ContainsKey(r))
            {
                hasreqs = false;    //DCG does not contain this requirement, so we do not have all reqs
                if (expectations.ContainsKey(r))    //If we were already expecting something with id r, add that this e requires it
                    expectations[r].requirers.Add(e.id);
                else //If we weren't already expecting r, register it as a new expected element
                {
                    ExpectedElement exp = new ExpectedElement();
                    exp.id = r;
                    exp.requirers = new List<int>();
                    exp.requirers.Add(e.id);
                    expectations.Add(r, exp);
                }
            }
        }

        if (hasreqs)
            Create(e);
        else
            waiting.Add(e); //If we couldn't create e yet, put it in the waitlist
    }

    void Create(ElementPacket e)
    {
        //register the element in DCG
        //if this item is in the waitlist, remove it.
        //check if this element was expected, and if so check if we can create any elements it was expected by, recursively call create on each.
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
