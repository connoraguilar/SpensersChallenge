using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightThrow : MonoBehaviour {
    public StraightProj mProjectile;
	// Use this for initialization
	void Start () {
        Physics2D.IgnoreCollision(mProjectile.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
