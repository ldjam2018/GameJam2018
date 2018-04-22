using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToExplosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Collider>().enabled = false;
        GetComponent<Collider>().enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
//        Debug.Log("Collision");
        if (col.gameObject.tag == "Explosion")
        {
            Debug.Log("Apply Exlosion force");
             float radius = 25F;
             float power = 50.0F;
            GetComponent<Rigidbody>().AddExplosionForce(power, col.gameObject.transform.position, radius, 3.0F);
        }
    }
}
