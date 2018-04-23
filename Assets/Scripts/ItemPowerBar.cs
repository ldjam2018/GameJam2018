using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerBar : MonoBehaviour {


	private GameObject barFill;
	float powerBarFill = 0f;
	float numToFillBar = 15f;



    public float GetSpeed(float minSpeed,  float maxSpeed)
    {
        return Map(powerBarFill, 0, 15f, minSpeed, maxSpeed);
    }

    //maps a value from one range to another
    private float Map(float value, float originalMin, float originalMax, float newMin, float newMax)
    {
        return (newMin + (value - originalMin) * (newMax - newMin) / (originalMax - originalMin));
    }

    // Use this for initialization
    void Start () {
		barFill = this.transform.GetChild (0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void AddPower(float powerValue) {
//		Debug.Log ("Added " + powerValue);
		powerBarFill += (powerValue);
		if (powerBarFill > numToFillBar) {
			powerBarFill = numToFillBar;
		} else if (powerBarFill < 0) {
			powerBarFill = 0;
		}
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
