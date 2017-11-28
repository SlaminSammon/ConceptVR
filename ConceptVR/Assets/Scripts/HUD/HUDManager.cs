using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class HUDManager : MonoBehaviour
{
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

        // initialize HUDColor to blue
        HUDColor = new Color(0.0f, 0.15f, 1.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

        frame = leapcontroller.Frame();
        // if there are hands visible in the view.
        if (frame.Hands.Count > 0)
        {
            lHand = frame.Hands[0];
            // If it is the left hand, make the position of the HUD relative to the local position of the right index(bone3).
            if (lHand.IsLeft)
            {
                if (util.IsFlatHand(lHand))
                {
                    HandsUpDisplay.SetActive(true);
                }
                else
                {
                    HandsUpDisplay.SetActive(false);
                }
            }
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
                child.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 0.65f, 0.0f, 1.0f);
            }
            if (child.gameObject.name.Length > 4 && child.gameObject.name.Substring(child.gameObject.name.Length - 4) == "Text") {
                isTextObj = true;
                child.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            if (child.gameObject.name.Length > 5 && child.gameObject.name.Substring(child.gameObject.name.Length - 5) == "Frame") {
                isFrameObj = true;
            }
            if (!isFrameObj && !isButtonObj && !isTextObj) {
                // set the color of the child component to the HUDcolor
                child.gameObject.GetComponent<Renderer>().material.color = this.HUDColor;
            }
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
   

}
