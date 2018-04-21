using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerBar : MonoBehaviour {


	private GameObject barFill;
	float powerBarFill = 0f;
	float numToFillBar = 15f;


	// Use this for initialization
	void Start () {
		barFill = this.transform.GetChild (0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void AddPower(float powerValue) {
		Debug.Log ("Added " + powerValue);
		powerBarFill += powerValue;
		RefillPowerBar ();
	}

	public void RemovePower(float powerValue) {
		powerBarFill -= powerValue;
		if (powerBarFill < 0) {
			powerBarFill = 0;
		}

		RefillPowerBar ();
	}

	private void RefillPowerBar() {

		float barScale = powerBarFill/numToFillBar;

		if (barScale > 1) {
			barScale = 1;
		}

		barFill.transform.localScale = new Vector3 (1,barScale,1);
	}
}
