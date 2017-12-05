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
    public GameObject analogClock;
    public GameObject digitalClock;

    // current tool selected
    string toolSelected = "";

    Color HUDColor;
    Stack<HUDFrame> frameStack;
    List<HUDView> viewList;
    HandsUtil util;
    public HUDFrame mainFrame;

    // cooldown used between pressing buttons
    // cooldownTime stores the next time a cd will be ready
    float cooldown;
    float cooldownTime;


    public GameObject HandsUpDisplay;
    bool active;

    Leap.Controller leapcontroller;
    Leap.Frame frame;
    Leap.Hand lHand;
    public GameObject LeapHandController;

    // Use this for initialization
    void Start()
    {
        leapcontroller = new Leap.Controller();
        frame = leapcontroller.Frame();

        util = new HandsUtil();
        this.cooldown = 0.25f; // seconds per button press
        this.cooldownTime = Time.time; // current time is cooldown time

        // create the frameStack and initialize it with the mainFrame
        frameStack = new Stack<HUDFrame>();
        frameStack.Push(mainFrame);

        active = true;
        isAnalogClock = true;
        toolSelected = "PointTool";

        // initialize HUDColor to gray
        HUDColor = new Color(0.345f, 0.3568f, 0.3804f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        frame = leapcontroller.Frame();
        lHand = Hands.Left;
        // if there are hands visible in the view.
        if (lHand != null)
        {
            if (util.IsFlatHand(lHand))
            {
                HandsUpDisplay.SetActive(true);

                // main frame animation
                if (this.frameStack.Peek() == mainFrame && hudActive == false)
                {
                    hudActive = true;
                    mainFrame.GetComponent<AnimationObjects>().clock.GetComponent<Animator>().Play("analogclock");
                    mainFrame.GetComponent<AnimationObjects>().settingsButton.GetComponent<Animator>().Play("settingsbutton");
                    mainFrame.GetComponent<AnimationObjects>().prefabsButton.GetComponent<Animator>().Play("prefabsbutton");
                    mainFrame.GetComponent<AnimationObjects>().toolsButton.GetComponent<Animator>().Play("toolsbutton");
                }
            }
            else
            {
                HandsUpDisplay.SetActive(false);
                hudActive = false;
            }
            
        }
        else
        {
            // if no hands are visible in the view, set the HUD inactive
            //HandsUpDisplay.SetActive(false);
        }
    }

    public void Push(HUDFrame hudframe) {

        // deactivate current top level HUDframe
        this.frameStack.Peek().transform.gameObject.SetActive(false);
        // activate new top level HUDframe
        hudframe.gameObject.SetActive(true);
        // set all hudframe children to be active
        foreach (Transform child in hudframe.GetComponentsInChildren<Transform>()) {
            child.gameObject.SetActive(true);
        }
        updateColor(hudframe);

        this.frameStack.Push(hudframe);
    }

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
        }
    }

    public void updateToolButtonColor(string tool)
    {
        if (tool != null)
        {
            this.toolSelected = tool;
        }
        // set all tool buttons to hud color before updating the selected one to orange.
        GameObject.Find("DoodleButton").GetComponent<Renderer>().material.color = HUDColor;
        GameObject.Find("MoveButton").GetComponent<Renderer>().material.color = HUDColor;
        GameObject.Find("DestroyButton").GetComponent<Renderer>().material.color = HUDColor;
        GameObject.Find("PointButton").GetComponent<Renderer>().material.color = HUDColor;
        GameObject.Find("LinkButton").GetComponent<Renderer>().material.color = HUDColor;

        if (this.toolSelected == "DoodleTool")
        {
            GameObject.Find("DoodleButton").GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        if (this.toolSelected == "MoveTool")
        {
            GameObject.Find("MoveButton").GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        if (this.toolSelected == "DestroyTool")
        {
            GameObject.Find("DestroyButton").GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        if (this.toolSelected == "PointTool")
        {
            GameObject.Find("PointButton").GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        if (this.toolSelected == "LinkTool")
        {
            GameObject.Find("LinkButton").GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

    }


    public Color getHUDColor() {
        return HUDColor;
    }

    public void setHUDColor(Color color) {
        this.HUDColor = color;

        updateColor(this.frameStack.Peek());
    }

    public float getCooldownTime() {
        return this.cooldownTime;
    }

    public void setCooldownTime(float cdTime) {
        this.cooldownTime = cdTime;
    }

    public float getCooldown() {
        return this.cooldown;
    }

    public void setCooldown(float cd)
    {
        this.cooldown = cd;
    }


    public bool getActive()
    {
        return active;
    }

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
    }
}
