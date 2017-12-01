﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
public class HandsUtil {
    private static float dist = .015f;
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
        //return hand.PinchStrength > .9f;
        //first and foremost we need the index to be pinched, if it it is we have a true
        bool pinch = checkPinchOfFinger(hand, "index");
        //if we are in pinch with index check to see if other fingers are not.
        if (pinch)
        {
            string[] fings = { "ring", "pinky" };
            foreach(string f in fings)
            {
                //If any other finger is in pinch return false
                if (checkPinchOfFinger(hand, f))
                    return false;
            }
        }
        return pinch;
    }
    public bool IsGrabbing(Leap.Hand hand)
    {
        //return hand.GrabStrength > .9f;
        foreach(Leap.Finger f in hand.Fingers)
        {
            if (f.Type == Leap.Finger.FingerType.TYPE_THUMB)
                continue;
            if (!checkFingerGrip(f))
                return false;
        }
        return true;
    }
    public bool IsFlatHand(Leap.Hand hand)
    {
        float angle = Vector3.Angle(hand.PalmNormal.ToVector3(), Vector3.up);
        //Debug.Log(angle);
        return (hand.GrabAngle <= 1f) && (angle>105f && angle <130f);
    }
    public bool IsGrabbingAngle(Leap.Hand hand)
    {
        return hand.GrabAngle > Mathf.PI * 2f / 3f;
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
        Vector3 diff = thumb.TipPosition.ToVector3() - finger.TipPosition.ToVector3();
        float dist = diff.magnitude;
        Vector3 padDirection = Vector3.Cross(finger.Direction.ToVector3(), Vector3.Cross(finger.Direction.ToVector3(), finger.bones[1].Direction.ToVector3()));
        return dist * Mathf.Sign(Vector3.Project(diff, padDirection).x) * Mathf.Sign(padDirection.x);
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
        float mpAngle = 180 - Vector3.Angle(proximal-metacarpal, intermediate-proximal);
        float pdAngle = 180 - Vector3.Angle(intermediate-proximal, distal-intermediate);
        if (mpAngle < 120f && pdAngle < 120f)
            return true;
        return false;
    }
    public bool checkThumbsUp(Leap.Hand hand)
    {
        foreach(Leap.Finger f in hand.Fingers)
        {
            if (f.Type == Leap.Finger.FingerType.TYPE_THUMB)
            {
                if (f.IsExtended && checkDirectionUp(f))
                    continue;
                else return false;
            }
            else
            {
                if (f.IsExtended && checkDirectionUp(f))
                    return false;
                else continue;
            }
        }
        return true;
    }
    bool checkDirectionUp(Leap.Finger finger)
    {
        Vector3 direction = finger.Direction.ToVector3();
        return (direction.x > .95f && direction.y > .95f && direction.z > .95f);
    }
    public bool checkFingerGun(Leap.Hand hand)
    {
        if (!checkThumbsUp(hand))
            return false;
        if (!Hands.GetIndex(hand).IsExtended)
            return false;
        return true;
    }
    public bool checkTap(Queue<FrameInformation> frameQueue)
    {
        int count = frameQueue.Count;
        if (count < 11)
            return false;

        FrameInformation[] fArr = frameQueue.ToArray();
        float[] sharpness = new float[count];
        float[] padvel = new float[count];
        float[] accelMag = new float[count];
        for (int i = 1; i < count-1; ++i)
        {
            FingerInformation p = fArr[i - 1].index;
            FingerInformation n = fArr[i + 1].index;
            FingerInformation v = fArr[i].index;
            //Vector3 jerk = (n.tipVelocity - v.tipVelocity) - (v.tipVelocity - p.tipVelocity);
            //jerk = Vector3.Project(jerk, fArr[i].index.padDirection);
            Vector3 accel = (n.tipVelocity - p.tipVelocity);
            Vector3.Project(accel, fArr[i].index.padDirection);
            accelMag[i] = Mathf.Sign(Vector3.Project(accel, fArr[i].index.padDirection).x) * accel.x;
            padvel[i] = Vector3.Project(v.tipVelocity, fArr[i].index.padDirection).magnitude;
            padvel[i] *= Mathf.Sign(v.tipVelocity.x) * Mathf.Sign(fArr[i].index.padDirection.x);
            sharpness[i] = Vector3.Angle(v.tipPosition - p.tipPosition, n.tipPosition - v.tipPosition) * accel.sqrMagnitude;
        }
        
        if (sharpness[count - 2] > 20f && sharpness[count - 1] < sharpness[count - 2] && sharpness[count - 3] < sharpness[count - 2]
            && fArr[count-2].index.isExtended
            && accelMag[count-2] > 0)
        {
            return true;
        }
            
        return false;
    }
    public bool isSwiping(Leap.Hand hand, Queue<FrameInformation> framesQueue)
    {
        if (framesQueue.Count < 50) return false;
        FrameInformation[] frames = framesQueue.ToArray();
        if (Extended(hand.Fingers) >= 4)
        {
            Vector3 swipeDirection = new Vector3();
            swipeDirection = hand.PalmPosition.ToVector3() - frames[frames.Length - 1].hand.palmPosition;
            //Debug.Log("sd" + swipeDirection.x);
            /*if (hand.IsRight)
                swipeDirection = frames[frames.Length - 1].hand.palmPosition - hand.PalmPosition.ToVector3();
            else if (hand.IsLeft)
                swipeDirection = hand.PalmPosition.ToVector3() - frames[frames.Length-1].hand.palmPosition;
            else return false;*/

            string sDirection = "";

            float absX = Mathf.Abs(swipeDirection.x);
            float absY = Mathf.Abs(swipeDirection.y);
            float absZ = Mathf.Abs(swipeDirection.z);
            float handRoll = hand.PalmNormal.Roll;
           
            if (absX > absY && absX > absZ)
            {
                if (swipeDirection.x > .2f && handRoll > 0.5)
                    sDirection = "Right";
                else if (swipeDirection.x < .2f && handRoll < -0.5)
                    sDirection = "Left";
            }
            return ((hand.IsRight && sDirection == "Right") || (hand.IsLeft && sDirection == "Left")) ? true : false;
        }
        return false;
    }
    public int Extended(List<Leap.Finger> fingers)
    {
        int count = 0;
        foreach (Leap.Finger f in fingers)
            if (f.IsExtended)
                count++;
        return count;
    }
    public Leap.Hand getHandByHandedness(string handedness)
    {
        if (handedness == "Right")
            return Hands.Right;
        else if (handedness == "Left")
            return Hands.Left;
        else
            return null;
    }

}
