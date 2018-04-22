using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour {

    private bool isBoostActive;
    private bool isRocketActive;
    private float boostSpeed = 2f;
    private GameObject enemy;
    public GameObject rocket;


	// Use this for initialization
	void Start () {
        isBoostActive = false;
        isRocketActive = false;

        if (this.transform.parent.tag == "Player01")
        {
            enemy = GameObject.FindWithTag("Player02");
        }
        else
        {
            enemy = GameObject.FindWithTag("Player01");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

   public float UsePowerUp()
    {
        if (isBoostActive)
        {
            return boostSpeed;
        }
        else if (isRocketActive)
        {
            
            Vector3 playerPos = transform.position;
            Vector3 playerDirection = transform.forward;
            Quaternion playerRotation = transform.rotation;
            float spawnDistance = 10;

            Vector3 spawnPos = playerPos + playerDirection * spawnDistance +Vector3.up*2;

            GameObject[] rockets = {
            Instantiate(rocket, spawnPos + Vector3.right * 2, Quaternion.identity),
            Instantiate(rocket, spawnPos, Quaternion.identity),
            Instantiate(rocket, spawnPos + Vector3.left * 2, Quaternion.identity)
            };

            foreach (GameObject rocket in rockets){
                rocket.gameObject.GetComponent<Rocket>().SetTarget(enemy);
            }
        }
        return 1f;
    }

    //pickup
    void OnCollisionEnter(Collision col)
    {
       if (col.gameObject.tag == "BoostPickup")
        {
            isBoostActive = true;
            isRocketActive = false;

            //call anything on powerup script if needed
            col.gameObject.GetComponent<PowerUp>().Explode();   
            
        }

        if (col.gameObject.tag == "RocketPickup")
        {
            isBoostActive = false;
            isRocketActive = true;

            //call anything on powerup script if needed            
            col.gameObject.GetComponent<PowerUp>().Explode();

        }
    }



}
