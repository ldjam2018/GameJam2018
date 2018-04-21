using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody rigidBody;

	private float xAxis, yAxis;

	public float movementSpeed = 7;


	// Use this for initialization
	void Start () {
		this.rigidBody = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {

		xAxis = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.LeftStickX);

		yAxis = XboxCtrlrInput.XCI.GetAxis (XboxCtrlrInput.XboxAxis.LeftStickY);

		Debug.Log("xAxis: " + xAxis);
		Debug.Log("yAxis: " + yAxis);

	}

	private void FixedUpdate() {
		this.rigidBody.velocity = new Vector2(xAxis, -yAxis);

	}


}
