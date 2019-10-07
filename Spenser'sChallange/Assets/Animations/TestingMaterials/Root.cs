using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour {
    public Joint[] joints;
    public GameObject target;
	// Use this for initialization
	void Start () {
       joints= GetComponentsInChildren<Joint>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
