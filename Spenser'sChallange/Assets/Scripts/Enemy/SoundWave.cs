using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour {
    public float lifespan = 3;
    private float existTime = 0;
    private Vector3 initalScale;
    private bool isGrowing;
    private bool exists = false;
	// Use this for initialization
	void Start () {
        initalScale = transform.localScale;
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isGrowing)
        {
            
            Vector3 neoScale = transform.localScale;
            neoScale.x += Time.deltaTime;
            neoScale.y += Time.deltaTime;
            transform.localScale = neoScale;
            existTime += Time.deltaTime;
            if(existTime>= lifespan)
            {
                Despawn();
            }
        }
	}

    void Despawn()
    {
        transform.localScale = initalScale;
        existTime = 0;
        isGrowing = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Spawn()
    {
        isGrowing = true;
        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public bool Exists()
    {
        return isGrowing;
    }
}
