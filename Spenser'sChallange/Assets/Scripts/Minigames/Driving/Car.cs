using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    private Vector3 direction;
    private float mSpeed;
    private float mLifeSpan;
    private float currentLife = 0;
    private float yRotation;
    private bool isFrozen = true;
    private CarSpawner mSpawner;
	// Use this for initialization
	void Start () {
        mSpawner = GetComponentInParent<CarSpawner>();
        mSpeed = mSpawner.carSpeed;
        mLifeSpan = mSpawner.lifeSpan;
        GetComponent<Collider>().enabled = false;
        direction = transform.forward;
        yRotation =mSpawner.transform.rotation.eulerAngles.y;
        //Matrix4x4 rotationMatrix = Matrix4x4.identity;
        //rotationMatrix.SetRow(0, new Vector4(Mathf.Cos(yRotation), 0, Mathf.Sin(yRotation)));
        //rotationMatrix.SetRow(1, new Vector4(0, 1, 0));
        //rotationMatrix.SetRow(2, new Vector4(-Mathf.Sin(yRotation), 0, Mathf.Cos(yRotation)));
        //direction = rotationMatrix.MultiplyVector(direction);
    }
	
	// Update is called once per frame
	void Update () {
        if(!isFrozen)
            UpdatePosition();
        currentLife += Time.deltaTime;
        if(currentLife >= mLifeSpan)
        {
            transform.position = mSpawner.transform.position;
            GetComponent<MeshRenderer>().enabled = false;
            isFrozen = true;
            currentLife = 0;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
	}

    void UpdatePosition()
    {
        
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        if(yRotation == 0 || yRotation== 180)
        {
            velocity.z = Mathf.Sign(direction.z) * .4f * Time.deltaTime * mSpeed;
        }
        else
        {
            velocity.x = Mathf.Sign(direction.x) * .4f * Time.deltaTime * mSpeed;
        }
        //velocity.x += transform.forward.x * Time.deltaTime * mSpeed;
        //velocity.y += transform.forward.y * Time.deltaTime * mSpeed;
        
        GetComponent<Rigidbody>().velocity = velocity;
    }

    public void Freeze()
    {
        isFrozen = true;
    }
    public void Drive()
    {
        if (isFrozen == true)
        {
            GetComponent<Collider>().enabled = true;
            isFrozen = false;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
