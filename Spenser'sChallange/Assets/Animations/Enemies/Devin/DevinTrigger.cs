using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevinTrigger : MonoBehaviour
{
    public GameObject mPlayer;
    public SpriteRenderer excPoint;
    public float speed;
    private float excTimer = 0;
    private bool startTimer = false;
    // Use this for initialization
    void Start()
    {
        excPoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer)
        {
            excTimer += Time.deltaTime;
            if(excTimer >= .5)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                transform.localScale = new Vector3(.75f, .75f, 1);
                GetComponent<Animator>().SetBool("isRunning", true);
                startTimer = false;
                excPoint.enabled = false;
            }
        }
        else if(excTimer >=.5)
        {
            Vector3 pos = transform.position;
            pos.x -= Time.deltaTime * speed;
            transform.position = pos;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Player"))
        {
            startTimer = true;
            excPoint.enabled = true;
        }
    }
}
