﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Checkpoint trigger. This is attached to every checkpoint node in the game.
/// </summary>
public class CheckpointTrigger : MonoBehaviour {

	public int checkpointNumber;
	public PositionTracker positionTracker;

	/// <summary>
	/// Iterate the vehicle's checkpoint number when it passes through the checkpoint
	/// </summary>
	void OnTriggerEnter (Collider other) {					//when a vehicle passes through the checkpoint
		if (other.tag == "Player01" || other.tag == "Player02") {														//if the vehicle is the player
			PlayerMovement playerScript = other.gameObject.GetComponent<PlayerMovement> ();
//			Debug.Log ("checkpointNumber: " + checkpointNumber + "PlayerScript.GetCurrentCheckpointNo(): " + playerScript.GetCurrentCheckpointNo());

			if (checkpointNumber == 0 && playerScript.GetCurrentCheckpointNo() == 20) {	//if completed a lap
				playerScript.IncrementLap ();												//increment the lap
				positionTracker.updateRacerLap (other.name);								//and update it in the position tracker
			}

			playerScript.SetCurrentCheckpointNo (checkpointNumber);							//then update the checkpoint number
			UpdateCheckpointPosition(other.gameObject, checkpointNumber);								//update the position tracker with the new checkpoint
		}
	}

	/// <summary>
	/// Updates the position tracker with the new checkpoint.
	/// </summary>
	public void UpdateCheckpointPosition(GameObject Racer, int checkpointNumber) {
		positionTracker.updateRacerCheckpoint(Racer.name, checkpointNumber);
	}
}
