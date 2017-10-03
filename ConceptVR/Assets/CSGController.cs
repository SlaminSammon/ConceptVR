using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sabresaurus.SabreCSG;

public class CSGController : MonoBehaviour {
    CSGModelBase csgModel;
    public Material mat;
    int count = 0;
	// Use this for initialization
	void Start () {
        csgModel = gameObject.AddComponent<CSGModelBase>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Random.value > 0.95f)
        {
            ++count;
            Debug.Log(count);
            Vector3 localPosition = Random.insideUnitSphere * 2;
            CSGMode csgMode = CSGMode.Add;
            if (Random.value > .5f)
                csgMode = CSGMode.Subtract;
            csgModel.CreateBrush(PrimitiveBrushType.Cube, localPosition, new Vector3(1f, 1f, 1f), Quaternion.identity, mat, csgMode);
        }
        csgModel.Build(false, false);
    }
}
