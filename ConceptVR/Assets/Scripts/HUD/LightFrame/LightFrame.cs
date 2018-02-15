using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFrame : HUDFrame {

    const float INTENSITY_SCALE = 15;
    HUDSlider intensitySlider;
    HUDSlider colorSlider;
    Color initialColor;
    Color lightColor;

	// Use this for initialization
	void Start () {
        intensitySlider = this.gameObject.transform.Find("IntensitySlider").GetComponent<HUDSlider>();
        colorSlider = this.gameObject.transform.Find("ColorSlider").GetComponent<HUDSlider>();

        // Color the ColorSlider
        var mesh = colorSlider.transform.Find("SlidingView").GetComponent<MeshFilter>().mesh;
        var uv = mesh.uv;
        var colors = new Color[uv.Length];
        // Instead if vertex.y we use uv.x
        for (var i = 0; i < uv.Length; i++)
            colors[i] = Color.Lerp(Color.red, Color.blue, uv[i].x);
        mesh.colors = colors;

    }
	
	// Update is called once per frame
	void Update () {
        foreach (LightItem light in ItemBase.sItems)
        {
            initialColor = light.GetComponent<Light>().color;

            light.GetComponent<Light>().intensity = intensitySlider.value * INTENSITY_SCALE;
            //light.GetComponent<Light>().color = lightColor;
            //light.selectedLightColor = lightColor;

//            light.selectedLightColor = initialColor;
        }
    }
}
