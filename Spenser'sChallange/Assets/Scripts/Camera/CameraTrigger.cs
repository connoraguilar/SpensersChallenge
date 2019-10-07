using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {

    public GameObject mCamera;
    public float distance;
    public int mWait;
    private int direction;
    private bool canTrigger = true;

    private bool wait;
    
	// Use this for initialization
	void Start () {
        direction = 1;
        wait = true;
	}
	
	// Update is called once per frame
	void Update () {
		//if(wait)
  //      {
  //          waitCounter += Time.deltaTime;
  //          if(waitCounter >= mWait)
  //          {
  //              wait = false;
  //              waitCounter = 0;
  //          }
  //      }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!mCamera.GetComponent<CameraFollow>().mFollowCamera && other.tag.Equals("Player") && canTrigger)
        {
            canTrigger = false;
            if (other.GetComponent<Rigidbody2D>().velocity.x != 0)
            {
                ShiftCameraPosition();
                other.GetComponent<PlayerMove>().FreezeMovement(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canTrigger = true;
    }

    public void ShiftCameraPosition()
    {
        if (wait)
        {
            
            float posSign = mCamera.GetComponent<CameraFollow>().mPlayer.transform.position.x - transform.position.x;
            if(posSign > 0 )
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            mCamera.GetComponent<CameraFollow>().ChangePosition(mCamera.transform.position.x + (direction * distance));
            //wait = true;
        }
    }
}
