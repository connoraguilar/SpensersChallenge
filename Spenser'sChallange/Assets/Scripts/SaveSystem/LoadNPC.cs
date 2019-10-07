using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNPC : MonoBehaviour {
    public List<NPC> npc;
	// Use this for initialization
	void Start () {
		for(int i =0; i< npc.Capacity;i++)
        {
           //NPCData data =  SaveSystem.LoadNPC(Application.persistentDataPath + "/" + npc[i].name + ".211");
            
           // if (data != null)
           // {
           //     npc[i].SetHasBeenTalkedTo(data.hasBeenTalkedTo);
           //     npc[i].SetLocked(data.locked);
           //     npc[i].SetTolerance(data.tolerance);
           // }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
