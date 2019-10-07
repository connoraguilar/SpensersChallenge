using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLock : MonoBehaviour {

    
    public GameObject lockedComponent;
    public int lockIndex;
    protected bool isLocked = false;

    // Use this for initialization
    public virtual void Start () {
        if (lockIndex < GameInformation.ObjectLocks.Count)
        {
            isLocked = GameInformation.ObjectLocks[lockIndex].locked;
        }
        if (isLocked && lockedComponent != null)
        {
            lockedComponent.SetActive(false); 
        }
        SaveSystem.SaveLock(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsLocked()
    {
        return isLocked;
    }
}
