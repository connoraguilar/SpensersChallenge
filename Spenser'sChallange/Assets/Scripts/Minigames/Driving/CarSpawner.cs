using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {
    public List<Car> mCars;
    public float carSpeed;
    public float lifeSpan;
    public float spawnTime;
    private float mSpawnTimer = 0;
    private int mCarIndex = 0;
	// Use this for initialization
	void Start () {
        SpawnCar();
        GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameInformation.isPaused)
        {
            mSpawnTimer += Time.deltaTime;
            if (mSpawnTimer >= spawnTime)
            {
                SpawnCar();
            }
        }
	}

    public void FreezeCars()
    {
        for(int i =0; i<mCars.Capacity;i++)
        {
            mCars[i].Freeze();
            mCars[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    void SpawnCar()
    {
        mCars[mCarIndex].Drive();
        mCarIndex++;
        mSpawnTimer = 0;
        if(mCarIndex == mCars.Capacity)
        {
            mCarIndex = 0;
        }
    }
}
