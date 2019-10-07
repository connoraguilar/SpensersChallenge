using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject mPlayer;
    public GameObject mEnemy;
    public float mVelocity;
    public float mThrowTime;
    public bool mFreeze = false;
    public float mThrowAngle;
    private float initialY;
    private float initialX;
    private float xVel;
    private float yVel;
    private float distance;
    private float time = 0;
    private bool isThrowing;
    private bool subtractX = false;
    private bool animating = false;
    private float cosAngle;
    private float sinAngle;
    private Vector3 lastPos;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        if (animating)
        {
            GetComponent<Animator>().Play("Beaker", -1, 0f);
            GetComponent<Animator>().enabled = true;
            animating = false;
        }
        else
        {
            lastPos = transform.position;
        }

        

        if (isThrowing)
        {
            time += Time.deltaTime;

            

            if (time <= mThrowTime)
            {
                if (!mFreeze)
                {
                   
                    Throw();
                }
            }
            else
            {
                mFreeze = false; 
                time = 0;
                isThrowing = false;
                Vector3 xyOffset = new Vector3(0.5f, 0, 0);
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Rigidbody2D>().simulated = false;
                transform.position = mEnemy.transform.position + xyOffset;
                
                GetComponent<Animator>().enabled = false;
                
            }
              
        }
        else
        {
           // Vector3 xyOffset= new Vector3(0.5f,0,0);
            //transform.position = mEnemy.transform.position + xyOffset;
        }
	}

    void Throw()
    {
        Vector3 newPos = transform.position;
        if(subtractX)
            newPos.x = initialX - (xVel * time * Mathf.Cos(mThrowAngle));
        else
            newPos.x = initialX + (xVel * time * Mathf.Cos(mThrowAngle));

        newPos.y = initialY + (yVel * time * Mathf.Sin(mThrowAngle)) + (0.5f * Physics2D.gravity.y * Mathf.Pow(time, 2));
       // Debug.Log("y equation: " + (mVelocity * time * Mathf.Sin(sinAngle)) + " " + (0.5f * Physics2D.gravity.y * Mathf.Pow(time, 2)));
        //Debug.Log("y pos: " + newPos.y + " x pos: " + newPos.x);
        transform.position = newPos;
        animating = true;

       // Debug.Log(newPos.x + " " + newPos.y);
    }

    public void StartThrow()
    {
        if (!isThrowing && !GameInformation.isPaused)
        {
          
            initialY = transform.position.y;
            initialX = transform.position.x;

            isThrowing = true;

            Vector3 projToPlayer = mPlayer.transform.position - transform.position;
            distance = Vector3.Distance(mPlayer.transform.position, transform.position);
            mVelocity =  distance / (Mathf.Sin(2 * mThrowAngle * Mathf.Deg2Rad) / 9.8f);
            xVel = Mathf.Sqrt(mVelocity) * Mathf.Cos(mThrowAngle * Mathf.Deg2Rad);
            yVel = Mathf.Sqrt(mVelocity) * Mathf.Cos(mThrowAngle * Mathf.Deg2Rad);
            mThrowTime = distance / xVel;

            projToPlayer.Normalize();

            if (initialX > mPlayer.transform.position.x)
            {
                subtractX = true;
            }
            else
            {
                subtractX = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(col.gameObject.tag);
        //if (GetComponent<Rigidbody2D>().velocity.y == 0 && col.gameObject.tag.Equals("Platforms"))
        //{
        Debug.Log("eh");
        
       // GetComponent<Animator>().Play("Beaker", -1, 0f);
        //}
    }
}
