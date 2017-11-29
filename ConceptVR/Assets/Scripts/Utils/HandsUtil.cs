using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
public class HandsUtil {
    private static float dist = .025f;
    //Gets the position of the fingers on a hand
    public Vector3 getIndexPos(Leap.Hand hand)
    {
        return Hands.GetIndex(hand).TipPosition.ToVector3();
    }
    public Vector3 getMiddlePos(Leap.Hand hand)
    {
        return Hands.GetMiddle(hand).TipPosition.ToVector3();
    }
    public Vector3 getRingPos(Leap.Hand hand)
    {
        return Hands.GetRing(hand).TipPosition.ToVector3();
    }
    public Vector3 getPinkyPos(Leap.Hand hand)
    {
        return Hands.GetPinky(hand).TipPosition.ToVector3();
    }
    public Vector3 getThumbPos(Leap.Hand hand)
    {
        return Hands.GetThumb(hand).TipPosition.ToVector3();
    }

    //Check pinch and grab pose
    public bool IsPinching(Leap.Hand hand)
    {
        return hand.PinchStrength > .9f;
        //first and foremost we need the index to be pinched, if it it is we have a true
        /*bool pinch = checkPinchOfFinger(hand, "index");
        //if we are in pinch with index check to see if other fingers are not.
        if (pinch)
        {
            string[] fings = { "middle", "ring", "pinky" };
            foreach(string f in fings)
            {
                //If any other finger is in pinch return false
                if (checkPinchOfFinger(hand, f))
                    return false;
            }
        }
        return pinch;*/
    }
    public bool IsGrabbing(Leap.Hand hand)
    {
        //return hand.GrabStrength > .9f;
        foreach(Leap.Finger hand.fingers)
    }
    public bool IsFlatHand(Leap.Hand hand)
    {
        return hand.GrabAngle <= 1f;
    }
    public bool IsGrabbingAngle(Leap.Hand hand)
    {
        return hand.GrabAngle > 3f;
    }
    public bool checkPinchOfFinger(Leap.Hand hand, string finger)
    {
        switch (finger)
        {
            case "index":
                if (getPinchDistance(hand.GetIndex(), hand.GetThumb()) < dist)
                    return true;
                break;
            case "middle":
                if (getPinchDistance(hand.GetMiddle(), hand.GetThumb()) < dist)
                    return true;
                break;
            case "ring":
                if (getPinchDistance(hand.GetRing(), hand.GetThumb()) < dist)
                    return true;
                break;
            case "pinky":
                if (getPinchDistance(hand.GetPinky(), hand.GetThumb()) < dist)
                    return true;
                break;
            default:
                break;
        }
        return false;
    }
    public float getPinchDistance(Leap.Finger finger, Leap.Finger thumb)
    {
        return Vector3.Distance(finger.TipPosition.ToVector3(),thumb.TipPosition.ToVector3());
    }
    public Vector3 getMetacarpalPosition(Leap.Finger finger)
    {
        return finger.Bone(Leap.Bone.BoneType.TYPE_METACARPAL).Center.ToVector3();
    }
    public Vector3 getProximalPosition(Leap.Finger finger)
    {
        return finger.Bone(Leap.Bone.BoneType.TYPE_PROXIMAL).Center.ToVector3();
    }
    public Vector3 getIntermediatePosition(Leap.Finger finger)
    {
        return finger.Bone(Leap.Bone.BoneType.TYPE_INTERMEDIATE).Center.ToVector3();
    }
    public Vector3 getDistalPosition(Leap.Finger finger)
    {
        return finger.Bone(Leap.Bone.BoneType.TYPE_DISTAL).Center.ToVector3();
    }
    public bool checkFingerGrip(Leap.Finger finger)
    {
        Vector3 distal = getDistalPosition(finger);
        Vector3 proximal = getProximalPosition(finger);
        Vector3 intermediate = getIntermediatePosition(finger);
        Vector3 metacarpal = getMetacarpalPosition(finger);
        float mpAngle = Vector3.Angle(proximal-metacarpal, intermediate-proximal);
        float pdAngle = Vector3.Angle(intermediate-proximal, distal-intermediate);
        if (mpAngle < 25f && pdAngle < 25f)
            return true;
        return false;
    }

}
