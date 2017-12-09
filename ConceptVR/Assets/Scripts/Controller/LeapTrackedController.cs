using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Leap.Unity;
using Leap;

public struct FrameInformation
{
    public bool grabHeld;
    public bool pinchHeld;
    public HandInformation hand;
    public FingerInformation thumb;
    public FingerInformation index;
    public FingerInformation middle;
    public FingerInformation ring;
    public FingerInformation pinky;
}
public struct FingerInformation
{
    public bool isExtended;
    public Vector3 direction;
    public Vector3 padDirection;
    public Vector3 tipPosition;
    public Vector3 tipVelocity;
}
public struct HandInformation
{
    public int velocityChange;
    public Vector3 palmVelocity;
    public Vector3 palmPosition;
    public Vector3 palmNormal;
    public Vector3 direction;
    public float pitch;
    public float yaw;
    public float roll;
    public Quaternion rotation;
}
public delegate void GestureEventHandler();
public delegate void GesturePositionEventHandler(Vector3 position);
public class LeapTrackedController : MonoBehaviour
{
    private Leap.Hand hand;
    private HandsUtil util;
    private LeapServiceProvider leapProvider;
    public bool pinchHeld = false;
    bool flippedPinch = false;
    public bool pinchInput = false;
    public bool gripInput = false;
    bool flippedGrab = false;
    public bool grabHeld = false;
    public bool swipe = false;
    public int velocity = 1; //To be checked against a seperate frame. 0 means decreased velocity. 1 means stagnent. 2 means increased.
    public Vector3 position;
    public string handedness;
    public int toolIndex = 0;
    public int swipeCount = 0;
    public static float cooldown = 1.25f;
    public static float tapCooldown = .25f;
    public float swipeCooldownTime;
    public float tapCooldownTime;
    public event GestureEventHandler pinchMade;
    public event GestureEventHandler pinchGone;
    public event GestureEventHandler grabMade;
    public event GestureEventHandler grabGone;
    public event GestureEventHandler swipeMade;
    public event GesturePositionEventHandler tapMade;
    public int heldFrames = 75;
    public Queue<FrameInformation> frameQueue;
    public FrameInformation currentFrame;

    // Use this for initialization
    void Start()
    {
        util = new HandsUtil();
        frameQueue = new Queue<FrameInformation>();
        currentFrame = new FrameInformation();
        tapCooldownTime = Time.time;
        swipeCooldownTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        removeExtraHands(); //Recent test found that a third hand can enter scene. Gets that outta there
        //Grab logic. Currently deprecated. May come back.
        bool grab = checkGrab();
        if (!grab)
        {
            grab = checkRecentGrabData();
            if (grab)
                flippedGrab = true;
        }
        //begin checking pinch logic
        bool pinch = false;
        swipe = false;
        if (!grab)
        {
            Debug.Log("Not Grabbing");
            OnGrabGone();
            pinch = checkPinch();
            //if pinch is false, chyeck recent frames to determine if its a leap motion hiccup
            if (!pinch)
            {
                pinch = checkRecentPinchData();
                if (pinch) flippedPinch = true;
            }
        }
        if(!pinch) OnPinchGone();
        //Final grab check
        if (grab && !grabHeld && !flippedGrab)
        {
            Debug.Log("Grabbing");
            OnGrabHeld();
        }
        //Final pinch check
        else if(pinch && !pinchHeld && !flippedPinch)
        {
            OnPinchHeld();
        }
        //Begin swipe check
        if (checkSwipe())
        {
            //We have a cooldown so swipes cannot be spammed
            if (Time.time > swipeCooldownTime)
            {
                swipe = true;
                EditorApplication.Beep();
                swipeMade();
                swipeCooldownTime = Time.time + cooldown;
            }
            else
                Debug.Log("Cooldown!!");
        }
        if (checkTap() && !swipe)
        {
            if (Time.time > tapCooldownTime)
            {
                Debug.Log(tapMade);
                tapMade(frameQueue.ToArray()[frameQueue.Count - 2].index.tipPosition);
                tapCooldownTime = Time.time + tapCooldown;
            }
        }
        if (flippedPinch)
        {
            pinchHeld = false;
            flippedPinch = false;
            pinchInput = true;
        }
        else
            pinchInput = false;
        if (flippedGrab)
        {
            grabHeld = false;
            flippedPinch = false;
            gripInput = true;
        }
        else
            gripInput = false;
        if (hand != null)
        {
            currentFrame = fetchFrameInformation();
            frameQueue.Enqueue(currentFrame);
            if (frameQueue.Count > heldFrames)
                frameQueue.Dequeue();
        }
     

    }

    public void OnPinchHeld()
    {
        pinchHeld = true;
        if (pinchMade != null)
            pinchMade();
    }
    public void OnPinchGone()
    {

        pinchHeld = false;
        if (pinchGone != null)
            pinchGone();
    }
    public void OnGrabHeld()
    {
        grabHeld = true;
        if (grabMade != null)
            grabMade();
    }
    public void OnGrabGone()
    {
        grabHeld = false;
        if (grabGone != null)
            grabGone();
    }





    public bool checkPinch()
    {
        //Determine which hand.
        if(handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        //Check if hand exists
        if (hand == null)
            return false;
        //Get position and value of Pinch
        if(!flippedPinch)
            position = util.weightedPos(hand);
        return util.IsPinching(hand);
    }
    public bool checkGrab()
    {
        if (handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        if (hand == null)
            return false;
        position = util.getIndexPos(hand);
        return util.IsGrabbing(hand);
        
    }
    //in progress
    public bool checkThumbsUp()
    {
        if (handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        if (hand == null)
            return false;
        position = util.getThumbPos(hand);
        return util.checkThumbsUp(hand);
    }

    public bool checkTap()
    {
        if (handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        if (hand == null)
            return false;
        position = util.getIndexPos(hand);
        bool ret = util.checkTap(frameQueue, hand);
        return ret;
    }
    public bool checkSwipe()
    {
        if (handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        if (hand == null)
            return false;
        position = util.getIndexPos(hand);
        return util.isSwiping(hand, frameQueue);
    }
    FrameInformation fetchFrameInformation()
    {
        FrameInformation frameInfo = new FrameInformation();
        frameInfo.grabHeld = grabHeld;
        frameInfo.pinchHeld = pinchHeld;
        foreach(Leap.Finger f in hand.Fingers)
        {
            FingerInformation fingerInfo = new FingerInformation();
            fingerInfo.isExtended = f.IsExtended;
            fingerInfo.direction = f.Direction.ToVector3();
            fingerInfo.padDirection = Vector3.Cross(fingerInfo.direction, Vector3.Cross(fingerInfo.direction, f.bones[1].Direction.ToVector3()));
            fingerInfo.tipPosition = f.TipPosition.ToVector3();
            fingerInfo.tipVelocity = f.TipVelocity.ToVector3();
            switch (f.Type)
            {
                case Leap.Finger.FingerType.TYPE_INDEX:
                    frameInfo.index = fingerInfo;
                    break;
                case Leap.Finger.FingerType.TYPE_MIDDLE:
                    frameInfo.middle = fingerInfo;
                    break;
                case Leap.Finger.FingerType.TYPE_RING:
                    frameInfo.ring = fingerInfo;
                    break;
                case Leap.Finger.FingerType.TYPE_PINKY:
                    frameInfo.pinky = fingerInfo;
                    break;
                case Leap.Finger.FingerType.TYPE_THUMB:
                    frameInfo.thumb = fingerInfo;
                    break;
            }
        }
        if(handedness == "Right")
            hand = Hands.Right;
        else
            hand = Hands.Left;
        HandInformation handInfo = new HandInformation();
        handInfo.direction = hand.Direction.ToVector3();
        handInfo.pitch = hand.Direction.Pitch;
        handInfo.roll = hand.Direction.Roll;
        handInfo.yaw = hand.Direction.Yaw;
        handInfo.palmPosition = hand.PalmPosition.ToVector3();
        handInfo.palmVelocity = hand.PalmVelocity.ToVector3();
        handInfo.palmNormal = hand.PalmNormal.ToVector3();
        handInfo.rotation = hand.Rotation.ToQuaternion();
        frameInfo.hand = handInfo;
        return frameInfo;

    }
    public bool checkRecentPinchData()
    {
        if (frameQueue.Count < 50) return false;
        FrameInformation[] frames = frameQueue.ToArray();
        for(int i = frames.Length-1; i > frames.Length-5; --i)
        {
            if (frames[i].pinchHeld)
                return true;
        }
        return false;
    }
    public bool checkRecentGrabData()
    {
        if (frameQueue.Count < 50) return false;
        FrameInformation[] frames = frameQueue.ToArray();
        for (int i = frames.Length - 1; i > frames.Length - 5; --i)
        {
            if (frames[i].grabHeld)
                return true;
        }
        return false;
    }
    public void removeExtraHands()
    {
        GameObject go = GameObject.Find("RigidRoundHand_L(Clone)");
        if (go != null) GameObject.Destroy(go);
        go = GameObject.Find("RigidRoundHand_R(Clone)");
        if(go != null) GameObject.Destroy(go);
        go = GameObject.Find("LoPoly_Rigged_Hand_Left(Clone)");
        if (go != null) GameObject.Destroy(go);
        go = GameObject.Find("LoPoly_Rigged_Hand_Right(Clone)");
        if (go != null) GameObject.Destroy(go);
    }
}
