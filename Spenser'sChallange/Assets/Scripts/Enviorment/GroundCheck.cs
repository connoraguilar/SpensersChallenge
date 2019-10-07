using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ContactFilter2D enemyFilter = new ContactFilter2D();
        enemyFilter.useLayerMask = true;
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        enemyFilter.SetLayerMask(enemyLayer);
        Collider2D[] results = new Collider2D[5];
        for(int i =0; i< 5;i++)
        {
            results[i] = null;
        }
        
        GetComponent<Rigidbody2D>().OverlapCollider(enemyFilter, results);

        if (results[0] != null)
        {
           
            for(int i =0; i< results.Length;i++)
            {
                if (results[i] != null)
                {
       
                    if (results[i].tag.Equals("Projectile"))
                    {
                        results[i].GetComponent<Projectile>().mFreeze =true;
                    }
                }
            }
        }
    }
}
