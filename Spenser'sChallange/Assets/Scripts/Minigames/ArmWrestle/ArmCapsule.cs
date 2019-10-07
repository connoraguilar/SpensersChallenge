using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCapsule : MonoBehaviour {
    public GameObject defaultLocation;
    public GameObject otherCapsule;
    private bool isTouching = true;
    private float tolerance = 0.5f;
    private float time = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (otherCapsule != null)
        {
            tolerance += 0.1f;
           
            if (Mathf.Abs(transform.position.z - otherCapsule.transform.position.z) > 8)
            {
                isTouching = false;
            }
            if (!isTouching)
            {
                FixLocation();
            }
        }
        else
        {
            tolerance -= 0.05f;
            if(tolerance <= 0.5f)
            {
                tolerance = 0.5f;
            }
        }
        transform.position = defaultLocation.transform.position;
	}


    void FixLocation()
    {
        isTouching = true;
        List<GameObject> jointList = GetComponentInParent<ArmAnimation>().mJoints;
        int count = 0;
        //while (!isTouching || count >3)
        //{
        //    count++;      
        //    for(int i =0; i<jointList.Capacity;i++)
        //    {
        //        Vector3 eu = jointList[i].transform.rotation.eulerAngles;
        //        eu.x += 1;
        //        jointList[i].transform.rotation = Quaternion.Euler(eu);
        //    }
        //}
        //Debug.Log("fixing position " + time);
        Vector3 toOtherCapsule = otherCapsule.transform.position - transform.position;
        for (int i = 0; i < jointList.Capacity; i++)
        {
            Vector3 eu = jointList[i].transform.rotation.eulerAngles;
            eu.x -= Mathf.Sign(toOtherCapsule.z) *0.55f;
            jointList[i].transform.rotation = Quaternion.Euler(eu);
        }
    }
}
