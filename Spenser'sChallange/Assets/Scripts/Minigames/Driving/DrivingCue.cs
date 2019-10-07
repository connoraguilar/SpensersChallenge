using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingCue : MonoBehaviour {
    public TextAsset mScript;
    public GameObject carWheel;
    public string mText;
    public List<KeyCode> mKeyChoices;
	// Use this for initialization
	public void Start () {
        Debug.Log("contstse");
        for(int i=0; i<3;i++)
        {
            mKeyChoices.Insert(i, KeyCode.None);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void PerformFunction(KeyCode choice, GameObject player)
    {

    }
}
