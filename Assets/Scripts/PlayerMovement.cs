using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody rigidBody;

	private float xAxis, yAxis;

    private bool bumperDepressed=false;
    public float movementSpeed;
    public float minimumSpeed;
    public float maximumSpeed;
    public float rotationSpeed;
    public float boostBonus;
	private GameObject powerBar;

	public Text lapText;

	public XboxController controllerNumber;

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
        //		XCI.GetButton(XboxButton.RightBumper)
        return currentCheckpoint;
    }

    public void IncrementLap()
    {
		Debug.Log ("incrementing lap");
        if (lap < raceNumOfLaps)
        {   //if not on the final lap
            lap++;  //increment lap
			lapText.text = "Lap: " + (lap+1) + "/" + raceNumOfLaps;
                    //			GameObject.Find ("GUICanvas").GetComponent<GUITimer> ().UpdateLapsElapsedText (lap, raceNumOfLaps);	//update the GUI to display lap counter
        }
        else
        {   //else player has completed their final lap
            setMovementDisabled(true);  //disabled movement as player has finished

            //call GUI to display position and lap times
            //			GameObject.Find ("GUICanvas").GetComponent<GUITimer> ().completedRace ();
            //			GameObject.Find ("GUICanvas").GetComponent<GUITimer> ().racerFinished (gameObject.name);
        }
    }

    public void setMovementDisabled(bool isDisabled)
    {
        movementDisabled = isDisabled;
    }

    //------------------------------------------------------------------------

    // Use this for initialization
    void Start () {
		this.rigidBody = GetComponent<Rigidbody>();
        minimumSpeed = 60;
        maximumSpeed = 180;
        movementSpeed = 60;
        rotationSpeed = 120;
        powerBar = transform.GetChild(0).transform.GetChild(2).gameObject;
        boostBonus = 1f;

		lapText.text = "Lap: " + (lap+1) + "/" + raceNumOfLaps;

	}
	
	// Update is called once per frame
	void Update () {
        movementSpeed = powerBar.GetComponent<ItemPowerBar>().GetSpeed(minimumSpeed, maximumSpeed)*boostBonus;
		float rightTriggerPressed = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.RightTrigger, controllerNumber);
		float leftTriggerPressed = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.LeftTrigger, controllerNumber);

		if (leftTriggerPressed == 1f) {
			xAxis = 1 * movementSpeed;
		} else {
			xAxis = 0;
		}

		yAxis = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.LeftStickY, controllerNumber) * rotationSpeed;
        
		if (XCI.GetButton(XboxButton.RightBumper, controllerNumber) && bumperDepressed == false){
            bumperDepressed = true;
//            Debug.Log("Bumper");
            boostBonus = GetComponent<PlayerPowerUps>().UsePowerUp();
        }
        else
        {
            bumperDepressed = XCI.GetButton(XboxButton.RightBumper);
        }

		if (boostBonus > 1) {
			boostBonus -= 0.02f;
//			Debug.Log
		} else {
			boostBonus = 1f;
		}
        //		Debug.Log("xAxis: " + xAxis + " yAxis: " + yAxis + " rightTrigger: " + rightTriggerPressed + " leftTrigger: " + leftTriggerPressed + " Y: " + XboxCtrlrInput.XCI.GetButtonDown (XboxCtrlrInput.XboxButton.Y));
    }

	private void FixedUpdate() {
//		this.rigidBody.velocity = new Vector3(xAxis, 0,  0);
//		rigidBody.AddForce (new Vector3(transform.forward.x, 0 , transform.forward.z) * xAxis);
//		this.gameObject.transform.Rotate (new Vector3(yAxis,0,0));
	
		Vector3 movement = transform.rotation * Vector3.forward;	//calculate movement direction by rotation

		rigidBody.AddForce (xAxis  * Time.deltaTime * movement, ForceMode.Impulse);	

		transform.Rotate (Vector3.up, yAxis * Time.deltaTime);	//steering

	}
}
