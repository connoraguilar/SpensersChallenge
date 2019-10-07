using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject mPlayer;
    private Vector3 offsetFromPlayer;
    private Vector3 mRespawnPosition;
    public int panTime;
    private float desiredPosition;
    private float cameraOffset;
    private bool changePosition;
    public bool mFollowCamera;


    // Use this for initialization
    void Start () {
        offsetFromPlayer = transform.position - mPlayer.transform.position;
        changePosition = false;
        mRespawnPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (mFollowCamera)
        {
                Vector3 cameraTransform = transform.position;
                cameraTransform.x = mPlayer.transform.position.x + offsetFromPlayer.x;
                cameraTransform.z = mPlayer.transform.position.z + offsetFromPlayer.z;
                transform.position = cameraTransform;
        }
        else 
        {
            if (changePosition)
            {
                Vector3 newPosition = transform.position;
                newPosition.x += cameraOffset * panTime * Time.deltaTime;
                transform.position = newPosition;

                if (cameraOffset >= 0)
                {
                    if (transform.position.x >= desiredPosition)
                    {
                        Vector3 newPos = transform.position;
                        newPos.x = desiredPosition;
                        transform.position = newPos;
                        changePosition = false;
                       
                        mPlayer.GetComponent<PlayerMove>().FreezeMovement(false);
                    }
                }
                else
                {
                    if (transform.position.x <= desiredPosition)
                    {
                        Vector3 newPos = transform.position;
                        newPos.x = desiredPosition;
                        transform.position = newPos;
                        changePosition = false;

                        mPlayer.GetComponent<PlayerMove>().FreezeMovement(false);
                    }
                }
            }
        }
	}

    public void ChangePosition(float xPos)
    {
        if (!mFollowCamera)
        {
            desiredPosition = xPos;
            cameraOffset= Mathf.Sign(xPos - transform.position.x);
            changePosition = true;
        }
    }

    public void Respawn()
    {
        transform.position = mRespawnPosition;
    }

    public void SetRespawn(Vector3 respawn)
    {
        mRespawnPosition = respawn;
    }
}
