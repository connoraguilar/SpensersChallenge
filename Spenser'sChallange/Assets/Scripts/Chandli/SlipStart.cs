using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipStart : MonoBehaviour {


    public GameObject mPlayer;
    public GameObject mCamera;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        mPlayer.GetComponent<PlayerMove>().mSlipping = true;
        mCamera.GetComponent<CameraFollow>().mFollowCamera = true;
    }
}
