using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFrame : HUDFrame {

    const float SCALE = 10;
    HUDSlider scaleSlider;
    public GameObject LMHeadMountedRig;

	// Use this for initialization
	void Start () {
        scaleSlider = this.gameObject.transform.Find("ScaleSlider").GetComponent<HUDSlider>();

        // base.Start();
    }

    // Update is called once per frame
    void Update () {
        //transform.Find("Managers").GetComponent<SettingsManager>().scaleSlider = new Vector3(scaleSlider.value * SCALE + 1f, scaleSlider.value * SCALE + 1f, scaleSlider.value * SCALE + 1f);
        LMHeadMountedRig.transform.localScale = new Vector3(scaleSlider.value * SCALE + 1f, scaleSlider.value * SCALE + 1f, scaleSlider.value * SCALE + 1f);
        // base.Update();
    }
}
