using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy {
    public GameObject mProjectile;
    public float mShotLength;
    public float mThrowTime;
    private float mPauseTimer = 0;
    private bool startPause = false;
    private bool startThrowTimer;
    private float mThrowTimer;
    private Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        mProjectile.GetComponent<SpriteRenderer>().enabled = false;
        Physics2D.IgnoreCollision(mProjectile.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if(startPause)
        { 
            Vector2 newVel = GetComponent<Rigidbody2D>().velocity;
            newVel.x = 0;
            GetComponent<Rigidbody2D>().velocity = newVel;
            mPauseTimer += Time.deltaTime;
            if (mPauseTimer > 1.0)
            {
                mPauseTimer = 0;
                startPause = false;
                anim.SetBool("Throw", false);
               
            }
            if(mPauseTimer > 0.25)
            {
                mProjectile.GetComponent<SpriteRenderer>().enabled = true;
            }
            if(mPauseTimer > 0.4)
            {
                mProjectile.GetComponent<Rigidbody2D>().simulated = true;
            }
        }

        if(startThrowTimer)
        {
            mThrowTimer += Time.deltaTime;
            if(mThrowTimer > mThrowTime)
            {
                mThrowTimer = 0;
                startThrowTimer = false;
            }

        }
        else
        {
            Vector3 toPlayer = (mPlayer.GetComponent<Rigidbody2D>().transform.position - transform.position);
            float distanceToPlayer = toPlayer.magnitude;
            if (distanceToPlayer <= mShotLength && mProjectile != null)
            {
                mProjectile.GetComponent<Projectile>().StartThrow();
                startPause = true;
                startThrowTimer = true;
                anim.SetBool("Throw", true);
            }
        }


        


    }
}
