using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

  
    public GameObject mPlayer;
    public float mSpeed;
    public List<GameObject> mWaypoints;
    private int currentWaypoint;
    private Vector3 forward;
  
   
    // Use this for initialization
    void Start()
    {
        if (mWaypoints.Capacity > 0)
        {
            currentWaypoint = 0;
            forward = mWaypoints[0].transform.position - transform.position;
            forward.Normalize();
        }
    }

    // Update is called once per frame
    protected void Update()
    {

        if (mWaypoints.Capacity > 0)
        {
            Vector2 newVel = GetComponent<Rigidbody2D>().velocity;
            newVel.x = Mathf.Sign(forward.x) * mSpeed;
            GetComponent<Rigidbody2D>().velocity = newVel;


            //waypoint management

            //End of waypoint

            //Projectile - Should move to derived class

        }
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag.Equals("Waypoint") && mWaypoints.Count >0)
        {
            if (other.gameObject == mWaypoints[currentWaypoint])
            {
                currentWaypoint++;
                if (currentWaypoint >= mWaypoints.Count)
                {
                    currentWaypoint = 0;
                }
                forward = mWaypoints[currentWaypoint].transform.position - transform.position;
                forward.Normalize();
                if (Mathf.Sign(forward.x) > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }
    }
}
