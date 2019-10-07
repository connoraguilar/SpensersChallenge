using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWaveTeacher : Enemy {
    public SoundWave soundWave;
    public float attackTime = 3;
    private float timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if(timer >= attackTime)
        {
            soundWave.Spawn();
            timer = 0;
        }
        if(!soundWave.Exists())
        {
            timer += Time.deltaTime;
        }
	}
}
