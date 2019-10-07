using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingCue02 : DrivingCue
{

    // Use this for initialization
    public List<GameObject> mTransitionPoints;
    public float lerpTime;
    private float rotateAngle;
    private float currentAngle = 0;
    private float travelTime = 0;
    private Vector3 desiredPoint;
    private Vector3 currentPoint;
    private Vector3 toPoint;
    private bool startLerp = false;
    private GameObject mPlayer;
    void Start()
    {

        mKeyChoices.Insert(0, KeyCode.D);
        mKeyChoices.Insert(1, KeyCode.W);
        mText = mScript.text;

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

            if (rotateAngle != 0)
            {
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
            else
            {
                if (mPlayer.transform.position.z >= mTransitionPoints[2].transform.position.z)
                {
                    startLerp = false;
                    mPlayer.GetComponent<Drive>().SetHalting(false);
                }
            }

        }
    }

    public override void PerformFunction(KeyCode choice, GameObject player)
    {
        if (choice == KeyCode.A)
        {
            rotateAngle = -90;
            toPoint = mTransitionPoints[0].transform.position - transform.position;
        }
        else if (choice == KeyCode.D)
        {
            rotateAngle = 90;
            toPoint = mTransitionPoints[1].transform.position - transform.position;
        }
        else
        {
            rotateAngle = 0;
            toPoint = mTransitionPoints[2].transform.position - transform.position;
        }
        startLerp = true;
        mPlayer = player;

    }
}
