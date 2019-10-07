using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepInPlace : MonoBehaviour {
    public Vector3 location;
    public Vector3 scale;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position != location)
        {
            transform.position = location;
          
        }
        if(transform.localScale != scale)
        {
            transform.localScale = scale;
        }
	}
}
