using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class HUDManager : MonoBehaviour
{
    // active or inactive hud
    bool hudActive = false;
    // analog or digital clock
    bool isAnalogClock = false;

    // current tool selected
    string toolSelected = "";

    Color HUDColor;
    public Stack<HUDFrame> frameStack;
    List<HUDView> viewList;
    HandsUtil util;
    public GameObject frames;
    private HUDFrame mainFrame;

    [HideInInspector]
    public List<HUDFrame> framesList;

    Leap.Controller leapcontroller;
    Leap.Frame frame;
    Leap.Hand lHand;
    public GameObject LeapHandController;
    public GameObject HUDObject;
    public AnchorButton anchor;
    bool placed;

    public static HUDManager hudManager;

    // Use this for initialization
    void Start()
    {
        leapcontroller = new Leap.Controller();
        frame = leapcontroller.Frame();

        util = new HandsUtil();

        mainFrame = frames.transform.Find("MainFrame").GetComponent<HUDFrame>();

        // create the frameStack and initialize it with the mainFrame
        frameStack = new Stack<HUDFrame>();
        frameStack.Push(mainFrame);

        hudActive = true;
        isAnalogClock = true;

        // initialize HUDColor to gray
        HUDColor = new Color(0.345f, 0.3568f, 0.3804f, 1.0f);
        anchor.Anchor += Placement;
        placed = false;

        hudManager = this;

        framesList.Add(frames.transform.Find("ToolsFrame").GetComponent<HUDFrame>());
        framesList.Add(frames.transform.Find("PrefabsFrame").GetComponent<HUDFrame>());
        framesList.Add(frames.transform.Find("SettingsFrame").GetComponent<HUDFrame>());

    }

    // Update is called once per frame
    void Update()
    {
        if (placed)
            return;
        frame = leapcontroller.Frame();
        lHand = Hands.Left;
        // if there are hands visible in the view.
        if (lHand != null)
        {
            if (util.IsFlatHand(lHand))
                HUDObject.SetActive(true);
            else
                HUDObject.SetActive(false);
            
        }
        else
        {
            // if no hands are visible in the view, set the HUD inactive
            HUDObject.SetActive(false);
        }
    }

    public void Push(HUDFrame hudframe) {

        // deactivate current top level HUDframe
        if (!hudframe.isSubFrame)
            this.frameStack.Peek().transform.gameObject.SetActive(false);
        // activate new top level HUDframe
        hudframe.gameObject.SetActive(true);
        updateColor(hudframe);

        this.frameStack.Push(hudframe);
    }

    //Whateven is this function?  Who wrote this?
    public void Push(HUDFrame hudframe1, HUDFrame hudframe2) {
        this.frameStack.Push(hudframe2);
        this.frameStack.Push(hudframe1);
    }

    public void Pop() {

        HUDFrame removedFrame = this.frameStack.Peek();

        // never pop the mainFrame
        if (removedFrame != mainFrame)
        {
            // deactivate top level HUDFrame
            removedFrame.gameObject.SetActive(false);
            // pop top level HUDFrame from stack
            this.frameStack.Pop();
            // activate new top level HUDframe.
            this.frameStack.Peek().transform.gameObject.SetActive(true);
            updateColor(this.frameStack.Peek());
        }
    }

    void updateColor(HUDFrame topFrame) {

        foreach (Transform child in topFrame.GetComponentsInChildren<Transform>()) {
            bool isFrameObj = false;
            bool isButtonObj = false;
            bool isTextObj = false;

            /*
            // check for the child whose name is "BackButton"
            if (child.gameObject.name == "BackButton")
            {
                isButtonObj = true;
                // make BackButton orange
                child.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            if (child.gameObject.name.Length > 4 && child.gameObject.name.Substring(child.gameObject.name.Length - 4) == "Text") {
                isTextObj = true;
                // make text objects white
                child.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            if (child.gameObject.name.Length > 5 && child.gameObject.name.Substring(child.gameObject.name.Length - 5) == "Frame") {
                isFrameObj = true;
            }
            if (!isFrameObj && !isButtonObj && !isTextObj) {
                // set the color of the child component to the HUDcolor if it is not a tool button
                if (child.gameObject.name != "DrawButton" && child.gameObject.name != "DragButton" && child.gameObject.name != "BezierButton")
                    child.gameObject.GetComponent<Renderer>().material.color = this.HUDColor;
            }
            */
        }
    }

    public Color getHUDColor() {
        return HUDColor;
    }

    public void setHUDColor(Color color) {
        this.HUDColor = color;

        updateColor(this.frameStack.Peek());
    }
    /*
    public void changeClock()
    {
        this.isAnalogClock = !this.isAnalogClock;
        if (this.isAnalogClock)
        {
            analogClock.SetActive(true);
            digitalClock.SetActive(false);
        }
        else
        {
            analogClock.SetActive(false);
            digitalClock.SetActive(true);
        }
    }*/
    void Placement()
    {
        LeapTrackedController ltc = GameObject.Find("LoPoly_Rigged_Hand_Right").GetComponent<LeapTrackedController>();
        if (placed) {
            GameObject lHand = GameObject.Find("RigidRoundHand_L");
            if (lHand != null)
            {
                GameObject parent = lHand.transform.Find("palm").gameObject;
                GameObject HUD = GameObject.Find("HandsUpDisplay");
                HUD.transform.SetParent(parent.transform);
                HUD.transform.localPosition = new Vector3(0, 0, 0);
                HUD.transform.localRotation = new Quaternion(180, 0, 0, 0);
                placed = false;
                HUDObject.SetActive(false);
                ltc.hudAnchor = true;
            }
            else
            {
                //TODO: signal to the player they need to have their left hand visible
            }
        }
        else {
            GameObject.Find("HandsUpDisplay").transform.parent = null;
            placed = true;
            HUDObject.SetActive(true);
            ltc.hudAnchor = false;
        }
    }
}
