using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DCG Material", menuName = "DCGMaterial")]
public class DCGMaterial : ScriptableObject {

    public new string name;
    public new Material mat;
    public List<DCGMaterial> matList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
