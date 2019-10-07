using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devin : MonoBehaviour {
    public PlayerMove mChandli;
    private bool isRunning = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(mChandli.mSlipping)
        {
            Debug.Log(transform.position);
            StartRun();
        }
	}

    void StartRun()
    {
        Vector3 currPos = transform.position;
        currPos.x += mChandli.mSpeed * Time.deltaTime;
        transform.position = currPos;
    }
}
