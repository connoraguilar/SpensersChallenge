using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidProjectile : MonoBehaviour {
   // public bool throwing = false;
    public Transform mTarget;
    public float angle = 45;
    private float distanceToTarget;
    private float projVelocity;
    private float Vx;
    private float Vy;
    private float flightDuration;
    private float time = 0;
    private bool throwing = false;
    private int direction = 1;
    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(throwing)
        {
            transform.Translate(direction*Vx * Time.deltaTime, (Vy - (9.8f * time)) * Time.deltaTime, 0);
            time += Time.deltaTime;
        }
	}

    public void Throw()
    {
        distanceToTarget = Vector3.Distance(transform.position, mTarget.position);
        Vector3 dir = mTarget.position - transform.position;
        direction = (int)Mathf.Sign(dir.x);
        projVelocity = distanceToTarget / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / 9.8f);
        Vx = Mathf.Sqrt(projVelocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
        Vy = Mathf.Sqrt(projVelocity) * Mathf.Sin(angle * Mathf.Deg2Rad);
        flightDuration = distanceToTarget / Vx;
        throwing = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Pause(Vector3 pos)
    {
        transform.position = pos;
        GetComponent<SpriteRenderer>().enabled = false;
        throwing = false;
        time = 0;
    }
    public void UnPause()
    {
        throwing = true;
    }
}
