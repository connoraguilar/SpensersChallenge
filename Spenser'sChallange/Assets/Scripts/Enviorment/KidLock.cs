using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidLock : ObjectLock {
    public Enemy monitor;
	// Use this for initialization
	public override void Start () {
        base.Start();
        if(isLocked)
        {
            GetComponent<KidsNPC>().Enable();
            Destroy(monitor.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(!isLocked && monitor == null)
        {
            isLocked = true;
            SaveSystem.SaveLock(this);
            SaveSystem.SaveGame(GameInformation.player, GameInformation.playerPath);
        }
	}
}
