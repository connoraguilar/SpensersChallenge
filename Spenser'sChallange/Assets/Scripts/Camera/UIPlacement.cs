using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlacement : MonoBehaviour {

    public GameObject Text;
    private float xOffset;
    private float yOffset;
    // Use this for initialization
    void Start () {
        xOffset = transform.position.x - Text.transform.position.x;
        yOffset = transform.position.y - Text.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = Text.transform.position;
        newPos.x += xOffset;
        newPos.y += yOffset;
        transform.position = newPos;

	}
}
