using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProj : MonoBehaviour {
    public int direction;
    public int angle;
    public float existTime;
    public float respawnPeriod;
    public float speed;
    public Animator throwAnimation;
    private float lifeTime = 0;
    private float waitTime = 0;
    private Vector2 startLocation;
    private bool waiting = false;
	// Use this for initialization
	void Start () {
        startLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (lifeTime>=existTime)
        {
            lifeTime = 0;
            transform.position = startLocation;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            throwAnimation.SetBool("Throw", false);
            throwAnimation.SetTrigger("Idle");
            throwAnimation.SetBool("Idle", true);
            waiting = true;
        }

        if(waiting)
        {
            waitTime += Time.deltaTime;
            if(waitTime >= respawnPeriod)
            {
                waitTime = 0;
                Respawn();
            }
            else if(waitTime >= respawnPeriod - 0.5f)
            {
                throwAnimation.SetTrigger("Throw");

                throwAnimation.SetBool("Idle", false);
                throwAnimation.SetBool("Throw", true);
            }
        }
        else
        {
            lifeTime += Time.deltaTime;
            Vector2 pos = transform.position;

            if(angle==0)
            pos.x += Mathf.Sign(direction) * Time.deltaTime * speed;
            else
                pos.y += Mathf.Sign(direction) * Time.deltaTime * speed;
            transform.position = pos;
        }
	}

    void Respawn()
    {

        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        waiting = false;
    }
}
