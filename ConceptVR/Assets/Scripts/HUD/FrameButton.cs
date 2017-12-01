using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameButton : HUDButton {

    HUDManager HUD;
    public HUDFrame Frame;

    float cdTime;

    // Use this for initialization
    void Start()
    {
        HUD = GameObject.Find("Managers").GetComponent<HUDManager>();
        cdTime = HUD.getCooldownTime();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "bone3" || collision.gameObject.name == "bone2")
        {
            Leap.Unity.FingerModel finger = collision.gameObject.GetComponentInParent<Leap.Unity.FingerModel>();

            if (finger && finger.fingerType.ToString() == "TYPE_INDEX")
            {
                cdTime = HUD.getCooldownTime();

                if (Time.time > cdTime)
                {
                    HUD.Push(Frame);
                    HUD.setCooldownTime(Time.time + HUD.getCooldown());

                    if(Frame.gameObject.name == "SettingsFrame")
                    {
                        this.gameObject.GetComponent<AnimationObjects>().changeColorButton.GetComponent<Animator>().Play("changecolorbutton");
                        this.gameObject.GetComponent<AnimationObjects>().changeClockButton.GetComponent<Animator>().Play("changeclockbutton");

                    }
                    if (Frame.gameObject.name == "PrefabsFrame")
                    {
                        this.gameObject.GetComponent<AnimationObjects>().sphereModel.GetComponent<Animator>().Play("sphere");
                        this.gameObject.GetComponent<AnimationObjects>().capsuleModel.GetComponent<Animator>().Play("capsule");
                        this.gameObject.GetComponent<AnimationObjects>().cubeModel.GetComponent<Animator>().Play("cube");
                        this.gameObject.GetComponent<AnimationObjects>().cylinderModel.GetComponent<Animator>().Play("cylinder");
                    }
                    if (Frame.gameObject.name == "ToolsFrame")
                    {
                        HUD.updateToolButtonColor(null);
                        this.gameObject.GetComponent<AnimationObjects>().moveButton.GetComponent<Animator>().Play("movebutton");
                        this.gameObject.GetComponent<AnimationObjects>().destroyButton.GetComponent<Animator>().Play("destroybutton");
                        //this.gameObject.GetComponent<AnimationObjects>().bezierButton.GetComponent<Animator>().Play("bezeirbutton");
                        this.gameObject.GetComponent<AnimationObjects>().doodleButton.GetComponent<Animator>().Play("doodlebutton");
                        this.gameObject.GetComponent<AnimationObjects>().pointButton.GetComponent<Animator>().Play("pointbutton");
                        this.gameObject.GetComponent<AnimationObjects>().linkButton.GetComponent<Animator>().Play("linkbutton");

                    }

                }
            }
        }
    }
}
