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
    Dictionary<int, ElementPacket> waiting;


	void Start () {
        expectations = new Dictionary<int, ExpectedElement>();
        waiting = new Dictionary<int, ElementPacket>();

        if (isLocalPlayer)
            DCGBase.synch = this;
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
            waiting.Add(e.id, e); //If we couldn't create e yet, put it in the waitlist
    }

    void Create(ElementPacket packet)
    {
        switch(packet.type)
        {
            case ElementType.point:
                PointPacket p = packet as PointPacket;
                new Point(p.position, p.id);
                break;
            case ElementType.edge:
                List<Point> points = new List<Point>();
                foreach (int reqid in packet.requirements)
                    points.Add(DCGBase.all[reqid] as Point);
                new Edge(points, packet.id);
                break;
            case ElementType.face:
                List<Edge> edges = new List<Edge>();
                foreach (int reqid in packet.requirements)
                    edges.Add(DCGBase.all[reqid] as Edge);
                new Face(edges, packet.id);
                break;
            case ElementType.solid:
                List<Face> faces = new List<Face>();
                foreach (int reqid in packet.requirements)
                    faces.Add(DCGBase.all[reqid] as Face);
                new Solid(faces, packet.id);
                break;
        }
        
        if (waiting.ContainsKey(packet.id)) //if this item is in the waitlist, remove it.
            waiting.Remove(packet.id);
        
        //TODO: check if this element was expected, and if so check if we can create any elements it was expected by, recursively call create on each.
    }


    [Command]
    public void CmdAddPoint(int id, Vector3 position, int senderID)
    {
        RpcAddPoint(id, position, senderID);
        Debug.Log("Server Added Point");
    }

    [ClientRpc]
    public void RpcAddPoint(int id, Vector3 position, int senderID)
    {
        Debug.Log("Client Got Point");
        if (NetPlayer.local.playerID == senderID)
        {
            Debug.Log("But Client also made that point so fuckever I guess");
            return;
        }
        else
        {
            PointPacket ep = new PointPacket();
            ep.id = id;
            ep.requirements = new int[0];
            ep.type = ElementType.point;
            ep.position = position;
            Receive(ep);
        }
    }

    [Command]
    public void CmdAddElement(int id, int[] requirements, ElementType type, int senderID)
    {
        RpcAddElement(id, requirements, type, senderID);
        Debug.Log("Server Added Element");
    }

    [ClientRpc]
    public void RpcAddElement(int id, int[] requirements, ElementType type, int senderID)
    {
        Debug.Log("Client Got Element");
        if (NetPlayer.local.playerID == senderID)
            return;
        else
        {
            ElementPacket ep = new ElementPacket();
            ep.id = id;
            ep.requirements = requirements;
            ep.type = type;
            Receive(ep);
        }
    }
}
