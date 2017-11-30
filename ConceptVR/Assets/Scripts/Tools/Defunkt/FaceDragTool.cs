using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDragTool : Tool {

    public float grabDistance = .1f;
    Face nearestFace;
    List<Point> facePoints;
    List<Vector3> pointGrabPos;
    Vector3 conGrabPos;
    DCGBase dcgObject;

    // Use this for initialization
    void Start()
    {
        dcgObject = GameObject.Find("DCG").GetComponent<DCGBase>();
    }

    // Update is called once per frame
    void Update()
    {

        if (triggerInput)
        {
            if (nearestFace != null)
            {

                for (int i = 0; i < facePoints.Count; ++i)
                {
                    facePoints[i].setPosition(pointGrabPos[i] + (transform.position - conGrabPos));
                }
            }
        }
        else
        {
            float nDist2 = grabDistance * grabDistance;
            nearestFace = null;
            foreach (Face f in DCGBase.faces)
            {
                List<Point> fp = f.getPoints();   //face points
                List<Vector2> pp = new List<Vector2>();     //projected points
                Vector3 zvec = f.getNormal();
                Vector3 yvec = fp[1].position - fp[0].position;
                Vector3 grabProj3 = Vector3.ProjectOnPlane(transform.position, zvec);
                Vector3 grabProjY = Vector3.Project(grabProj3, yvec);
                Vector2 grab = new Vector2((grabProj3-grabProjY).magnitude, grabProjY.magnitude);

                //Vector3 grabRel = transform.position - f.
                float dist2 = 0;//(transform.position - Vector3.Project()).sqrMagnitude;

                if (dist2 > nDist2)
                    continue;

                foreach (Point p in fp)
                {
                    Vector3 proj3 = Vector3.ProjectOnPlane(p.position, zvec);
                    Vector3 projY = Vector3.Project(proj3, yvec);
                    pp.Add(new Vector2((proj3-projY).magnitude, projY.magnitude));   //TODO
                }


                //Check if the projected poly contains the projected controller
                float count = 0;
                Vector2 prev = pp[pp.Count - 1];
                foreach (Vector2 cur in pp)
                {
                    if ((prev.y > grab.y != cur.y > grab.y) && (grab.x < Mathf.Lerp(prev.x, cur.x, (grab.y - prev.y) / (cur.y - prev.y))))
                        count++;
                    prev = cur;
                }


                if (count % 2 == 1)
                {
                    nDist2 = dist2;//TODO
                    nearestFace = f;
                }
            }
        }
    }

    public override void TriggerDown()
    {
        if (nearestFace != null)
        {
            facePoints = nearestFace.getPoints();
            pointGrabPos = new List<Vector3>();
            foreach (Point p in facePoints)
                pointGrabPos.Add(p.position);
        }
        conGrabPos = transform.position;
        return;
    }

    public override void TriggerUp()
    {
        return;
    }

    public void OnRenderObject()
    {
        if (nearestFace != null)
        {
            dcgObject.faceMat.SetPass(0);
            
            Graphics.DrawMeshNow(nearestFace.mesh, Vector3.zero + nearestFace.getNormal() * .02f, Quaternion.identity);
            Graphics.DrawMeshNow(nearestFace.mesh, Vector3.zero - nearestFace.getNormal() * .02f, Quaternion.identity);
        }
    }
}
