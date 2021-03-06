﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour {

    private bool isBoostActive;
    private bool isRocketActive;
    private float boostSpeed = 4f;
    private GameObject enemy;
    public GameObject rocket;


	// Use this for initialization
	void Start () {
		isBoostActive = true;
        isRocketActive = false;

        if (this.transform.tag == "Player01")
        {
            enemy = GameObject.FindWithTag("Player02");
        }
        else
        {
            enemy = GameObject.FindWithTag("Player01");
        }
    }
	
	// Update is called once per frame
//	void Update () {
//		
//	}

   public float UsePowerUp()
    {
        if (isBoostActive)
        {
			isBoostActive = false;
            return boostSpeed;
        }
        else if (isRocketActive)
        {
			isRocketActive = false;
            Vector3 playerPos = transform.position;
            Vector3 playerDirection = transform.forward;
            Quaternion playerRotation = transform.rotation;
            float spawnDistance = 10;

            Vector3 spawnPos = playerPos + playerDirection * spawnDistance +Vector3.up*2;

			GameObject [] rockets = new GameObject[3]{
            Instantiate(rocket, spawnPos + Vector3.right * 2, Quaternion.identity),
            Instantiate(rocket, spawnPos, Quaternion.identity),
            Instantiate(rocket, spawnPos + Vector3.left * 2, Quaternion.identity)
            };

			Debug.Log (rockets.Length);

            foreach (GameObject rocket in rockets){
                rocket.gameObject.GetComponent<Rocket>().SetTarget(enemy);
				Debug.Log ("rocket target is: " + enemy.name);
            }
        }
        return 1f;
    }

    //pickup
    void OnCollisionEnter(Collision col)
    {
       if (col.gameObject.tag == "BoostPickup")
        {
			col.gameObject.GetComponent<Collider> ().enabled = false;
            isBoostActive = true;
            isRocketActive = false;

            //call anything on powerup script if needed
            col.gameObject.GetComponent<PowerUp>().Explode();   
            
        }

        if (col.gameObject.tag == "RocketPickup")
        {
			col.gameObject.GetComponent<Collider> ().enabled = false;

            isBoostActive = false;
            isRocketActive = true;

            //call anything on powerup script if needed            
            col.gameObject.GetComponent<PowerUp>().Explode();

        }
    }



}
