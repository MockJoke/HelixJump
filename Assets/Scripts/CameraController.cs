using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public BallController target;
    private float offset;       //keep initial distance between cam and ball

    private void Awake()        //just little earlier than start method 
    {
        offset = transform.position.y - target.transform.position.y;
    }

    void Update ()          //once per frame 
    {             
        //move camera smoothly to target height (yTargetPos)
        Vector3 curPos = transform.position;        //get the pos of cam
        curPos.y = target.transform.position.y + offset;        //always maintain distance to the ball
        transform.position = curPos;        //set the pos of cam
    }
}
