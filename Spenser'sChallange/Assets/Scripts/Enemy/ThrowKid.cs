using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKid : Enemy
{
    public KidProjectile projectile;
    public Animator throwAnimation;
    public float throwTime;
    public float pauseTime;
    private float throwTimer = 0;
    private bool isThrowing = false;
	// Use this for initialization
	void Start () {
        projectile.Throw();
        isThrowing = true;
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if(isThrowing )
        {
            throwTimer += Time.deltaTime;
            if(throwTimer>= throwTime)
            {
                isThrowing = false;
                throwTimer = 0;
                Debug.Log("pasue");
               
                throwAnimation.SetBool("Throw", false);
                projectile.Pause(transform.position);
            }
        }
        else
        {
 
            throwTimer += Time.deltaTime;
            if(throwTimer >= pauseTime)
            {
                throwTimer = 0;
                isThrowing = true;
              
                projectile.Throw();
            }
            else if(throwTimer >= pauseTime - 0.5f)
            {
                Debug.Log("throw");
                throwAnimation.SetBool("Throw", true);
            }
        }
	}
}
