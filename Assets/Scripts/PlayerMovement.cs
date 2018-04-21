using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody rigidBody;

	private float xAxis, yAxis;

	public float movementSpeed = 7;
	public float rotationSpeed = 2;


	// Use this for initialization
	void Start () {
		this.rigidBody = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
		float rightTriggerPressed = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.RightTrigger);
		float leftTriggerPressed = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.LeftTrigger);

		if (leftTriggerPressed == 1f) {
			xAxis = 1 * movementSpeed;
		} else {
			xAxis = 0;
		}

		yAxis = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.LeftStickY) * rotationSpeed;

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
