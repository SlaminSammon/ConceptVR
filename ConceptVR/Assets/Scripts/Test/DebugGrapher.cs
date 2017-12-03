using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGrapher : MonoBehaviour {
    public struct Graph
    {
        struct GraphValue
        {
            public float value;
            public bool isPOI;
            public GraphValue(float value, bool isPOI)
            {
                this.value = value;
                this.isPOI = isPOI;
            }
        }

        public LineRenderer line;
        Queue<GraphValue> values;
        public float yScale;
        public float xScale;

        public void Push(float value, bool isPOI, int length)
        {
            values.Enqueue(new GraphValue(value, isPOI));
            if (values.Count > length)
                values.Dequeue();
        }

        public void Update()
        {
            Vector3[] positions = new Vector3[values.Count];
            foreach (GraphValue gv in values)
            {
                
            }
            line.SetPositions(positions);
        }

        public void Render()
        {
            foreach (GraphValue gv in values)
            {
                if (gv.isPOI)
                    Graphics.DrawMeshNow(GeometryUtil.icoSphere2, )
            }
        }
    }

    public Dictionary<string, Graph> graphs;
    public int length;
    public float xScale;
    public float yScale;

    //Initiaize
	void Start () {
        //populate graphs
        foreach (KeyValuePair<string, Graph> kv in graphs)
        {
            for (int i = 0; i < length; ++i)
                kv.Value.Push(0, length);
            GameObject line = new GameObject();
            line.AddComponent<LineRenderer>();
            kv.Value.line = line.GetComponent<LineRenderer>();
        }
            
	}
	
	void Update () {
		
	}

    public void AddValue(string paramName, float value, bool isPOI)
    {

    }
}
