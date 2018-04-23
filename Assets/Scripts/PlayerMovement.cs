using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody rigidBody;

	private float xAxis, yAxis;

    private bool bumperDepressed=false;
    public float movementSpeed;				//the current max speed that can be reached according to how full the power bar is
	private float currentSpeed;				//the current speed of the car that is increased through acceleration
    public float minimumSpeed;
    public float maximumSpeed;				//max speed that can be reached when the power bar is filled
	public float maxReverseSpeed;
    public float rotationSpeed;
    public float boostBonus;
	private GameObject powerBar;
	private GameObject carModel;
	private float rotationTimer = 0f;

	public Text lapText;
	public Text pressStartToBeginText;

	public XboxController controllerNumber;

	private static int carsFinished = 0;

    //------------------------------------------------------------------------

    public int currentCheckpoint = 0;               //the last checkpoint that the car passed
    public int lap;                             //the current lap the racer is on
    public int raceNumOfLaps;                   //number of laps in the race

    private bool movementDisabled = false;

    /// <summary>
    /// Set the last checkpoint number the Player has passed.
    /// </summary>
    public void SetCurrentCheckpointNo(int checkpointNo)
    {
        currentCheckpoint = checkpointNo;
    }

    /// <summary>
    /// Returns the last checkpoint number the Player has passed.
    /// </summary>
    public int GetCurrentCheckpointNo()
    {
        return currentCheckpoint;
    }

    public void IncrementLap()
    {
		Debug.Log ("NextLap run Lap: " + lap + "/" + raceNumOfLaps);
        if (lap < raceNumOfLaps-1)
        {   //if not on the final lap
			Debug.Log ("incrementing lap");
            lap++;  //increment lap
			lapText.text = "Lap: " + (lap+1) + "/" + raceNumOfLaps;
        }
        else
        {   //else player has completed their final lap
			Debug.Log ("race over");

            setMovementDisabled(true);  //disabled movement as player has finished
			RaceComplete();
        }
    }

	private void RaceComplete() {
		if (carsFinished == 0) {
			transform.GetChild (2).GetChild (0).GetComponent<SpriteRenderer> ().enabled = true;
			carsFinished++;
		} else {
			transform.GetChild (2).GetChild (1).GetComponent<SpriteRenderer> ().enabled = true;
			carsFinished++;
		}

		pressStartToBeginText.text = "PRESS START TO RESTART";
	}

    public void setMovementDisabled(bool isDisabled)
    {
		
        movementDisabled = isDisabled;
    }

    //------------------------------------------------------------------------

    // Use this for initialization
    void Start () {
		this.rigidBody = GetComponent<Rigidbody>();
        minimumSpeed = 30;
        maximumSpeed = 70;
		currentSpeed = 0;
        movementSpeed = 20;
        rotationSpeed = 100;
		maxReverseSpeed = 15;
        powerBar = transform.GetChild(0).transform.GetChild(2).gameObject;
        boostBonus = 1f;

		carModel = transform.GetChild (1).gameObject;

		lapText.text = "Lap: " + (lap+1) + "/" + raceNumOfLaps;

		movementDisabled = true; 	//start with car disabled
	}
	
	// Update is called once per frame
	void Update () {

		if (!movementDisabled) {

			yAxis = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.LeftStickY, controllerNumber) * rotationSpeed;

			RollVehicle ();
			CalculateCurrentSpeed ();
			CheckIfPickupActivated ();



			if (boostBonus > 1) {
				boostBonus -= 0.02f;
			} else {
				boostBonus = 1f;
			}


		}

		if (XCI.GetButton (XboxButton.Start)) {
			SceneManager.LoadScene("MainScene");
		}
		//Debug.Log("xAxis: " + xAxis + " yAxis: " + yAxis + " rightTrigger: " + rightTriggerPressed + " leftTrigger: " + leftTriggerPressed + " Y: " + XboxCtrlrInput.XCI.GetButtonDown (XboxCtrlrInput.XboxButton.Y));
    }

	private void RollVehicle() {	//rolls the vehicle from side to side as you steer, and falls back to the centre when driving straight
		if (yAxis < 0f && rotationTimer < .3f) {

			carModel.transform.Rotate (0, -.2f, 0, Space.Self);
			rotationTimer += Time.deltaTime;
		}

		if (yAxis > 0f && rotationTimer > -.3f) {

			carModel.transform.Rotate (0, .2f, 0, Space.Self);
			rotationTimer -= Time.deltaTime;

		} 

		else if (yAxis == 0f && rotationTimer != 0) {
			if (rotationTimer > 0.02f) {
				carModel.transform.Rotate (0, .2f, 0, Space.Self);
				rotationTimer -= Time.deltaTime;
			} else if (rotationTimer < -0.02f) {
				carModel.transform.Rotate (0, -.2f, 0, Space.Self);
				rotationTimer += Time.deltaTime;
			}
		}
	}
	private void CalculateCurrentSpeed() {
		movementSpeed = powerBar.GetComponent<ItemPowerBar>().GetSpeed(minimumSpeed, maximumSpeed)*boostBonus;		//calculate current max speed that can be reached
//		float rightTriggerPressed = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.RightTrigger, controllerNumber);
		float leftTriggerPressed = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.LeftTrigger, controllerNumber);


		if (leftTriggerPressed == 1f && !(XCI.GetButton(XboxButton.LeftBumper, controllerNumber))) {				//if the accelerator is pressed (and not the reverse button)
			if (currentSpeed != movementSpeed) {												//if has not reached the max speed
				currentSpeed += (movementSpeed - currentSpeed) / 150;							//add to the speed of the car
				if (currentSpeed > movementSpeed) { 											//if adding exceeds the max speed - remove it
					currentSpeed = movementSpeed;
				}
			}
		} else if (leftTriggerPressed == 0f && !(XCI.GetButton(XboxButton.LeftBumper, controllerNumber))) {		//if the accelerator is NOT pressed (and not the reverse button)
			if (currentSpeed != 0) {															//if there is speed in the car's movement
				currentSpeed -= (currentSpeed) / 15;											//reduce the speed of the car
				if (currentSpeed < 0) { 														//if remove speed drops beneath 0 - reset to 0
					currentSpeed = 0;
				}
			}
		} else if (XCI.GetButton(XboxButton.LeftBumper, controllerNumber)) {			//if reverse button is pressed
			if (currentSpeed > -maxReverseSpeed) {						//if the current speed is higher than the max reversing speed
				if (currentSpeed > 15) {									//if still going at a good forward speed
					currentSpeed -= (currentSpeed) / 25;						//then effectively break

				} else {
					if (currentSpeed > -maxReverseSpeed) { 							//if speed drops below the max breaking speed - reset to maxBreakSpeed
						currentSpeed += -(maxReverseSpeed)/10;
					}
				}
			}
		}

		xAxis = 1 * currentSpeed;
	}

	private float CalculateRotationSpeed() {
		float thisRotSpeed = 0f;

		if (currentSpeed > 40) {
			thisRotSpeed = (float)40 / 50;
		} else {
			thisRotSpeed = currentSpeed / 50;
		}

		if (thisRotSpeed > .8f) {
			thisRotSpeed = .8f;
		}
		return thisRotSpeed;
	}

	private void CheckIfPickupActivated() {
		if (XCI.GetButton(XboxButton.RightBumper, controllerNumber) && bumperDepressed == false){
			bumperDepressed = true;
			boostBonus = GetComponent<PlayerPowerUps>().UsePowerUp();
		}
		else
		{
			bumperDepressed = false;
		}
	}
	private void FixedUpdate() {
		if (!movementDisabled) {
			Vector3 movement = transform.rotation * Vector3.forward;	//calculate movement direction by rotation

			rigidBody.AddForce (xAxis  * Time.deltaTime * movement, ForceMode.Impulse);	

			transform.Rotate (Vector3.up, (yAxis * CalculateRotationSpeed()) * Time.deltaTime);	//steering
		}
	}
}