using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public GameObject fireWorks;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Explode()
    {
        Instantiate(fireWorks, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
