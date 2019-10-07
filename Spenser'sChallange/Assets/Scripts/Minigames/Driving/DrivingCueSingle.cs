using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingCueSingle : DrivingCue {

    public GameObject mTransitionPoint;
    public float lerpTime;
    public float rotateAngle;
    private float currentAngle = 0;
    private float travelTime = 0;
    private Vector3 desiredPoint;
    private Vector3 currentPoint;
    private Vector3 toPoint;
    private bool startLerp = false;
    private GameObject mPlayer;

    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (startLerp)
        {
            Vector3 up = new Vector3(0, 1, 0);

            mPlayer.transform.Rotate(0, Time.deltaTime * rotateAngle, 0, Space.Self);


            Vector3 mPos = mPlayer.transform.position;
            //toPoint = desiredPoint - mPos;

            mPos += Time.deltaTime * lerpTime * toPoint;
            mPlayer.transform.position = mPos;

            currentAngle += rotateAngle * Time.deltaTime;
            if (Mathf.Sign(rotateAngle) > 0)
            {
                if (currentAngle > rotateAngle)
                {
                    mPlayer.GetComponent<Rigidbody>().MoveRotation(mPlayer.transform.rotation);
                    //Vector3 eu = transform.rotation.eulerAngles;
                    //eu.y += rotateAngle;
                    //Quaternion rotation = transform.rotation;
                    //rotation.eulerAngles = eu;
                    //mPlayer.transform.rotation = rotation;
                    startLerp = false;
                    currentAngle = 0;
                    mPlayer.GetComponent<Drive>().SetHalting(false);
                }
            }
            else
            {
                if (currentAngle < rotateAngle)
                {
                    startLerp = false;
                    currentAngle = 0;
                    mPlayer.GetComponent<Drive>().SetHalting(false);
                }
            }

        }
    }

    public override void PerformFunction(KeyCode choice,GameObject player)
    {
        toPoint = mTransitionPoint.transform.position - transform.position;
       
        startLerp = true;
        mPlayer = player;

    }
}
