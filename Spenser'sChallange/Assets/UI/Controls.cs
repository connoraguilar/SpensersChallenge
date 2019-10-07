using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    public Canvas controls;
    public Canvas overall;
    private GameObject overAll;
	// Use this for initialization
	void Start () {
        overAll = overall.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(GameInformation.isPaused == false)
        {
            controls.gameObject.SetActive(false);
        }
	}

    public void DisplayControls()
    {
        overall.gameObject.SetActive(false);
        controls.gameObject.SetActive(true);
    }

    public void HideControls()
    {
        controls.gameObject.SetActive(false);
        overall.gameObject.SetActive(true);

    }
}
