using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : Interactable {
    public Chandli player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        player.SpawnPosition = player.transform.position;
        SaveSystem.SaveGame(player, GameInformation.playerPath);
    }
}
