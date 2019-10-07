using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour {
    public PauseMenu pause;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UnPause()
    {
        pause.UnPause();
    }
}
