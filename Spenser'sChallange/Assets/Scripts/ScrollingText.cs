using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour {
    public float scrollSpeed;
    public Door levelChange;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = transform.position;
        newPos.y += scrollSpeed * Time.deltaTime;
        transform.position = newPos;
        if(newPos.y >= 45)
        {
            levelChange.Interact();
        }
	}
}
