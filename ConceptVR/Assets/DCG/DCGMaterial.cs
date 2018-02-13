using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DCG Material", menuName = "DCGMaterial")]
public class DCGMaterial : ScriptableObject {

    public new string name;
    public new Material mat;
    public List<Face> facesUsingMat = new List<Face>();

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
