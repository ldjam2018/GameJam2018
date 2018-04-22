using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


/// <summary>
/// The Position tracker class computes the position each racer is in, e.g. the leaderboard of who is is in first, second, down to last place. The
/// class also updates the graphics on the GUI to display this positioning.
/// </summary>
public class PositionTracker : MonoBehaviour {

	public List<GameObject> Racers;    									//list of racers;
	public int[] racerLapNumber /*= new int[4] {1,1,1,1}*/;				//the lap each racer is on
	public int[] racerCheckpointNumber /*= new int[4] {0,0,0,0}*/;		//the checkpoint each racer is on

	public Text player01Position;											//the text displaying the player's position e.g. 4th
	public Text player02Position;
	public Image firstPosition;											//the image in the GUI representing first place
	public Image secondPosition;
	public Image thirdPosition;
	public Image fourthPosition;

	public Sprite[] carSprites;


	private float nextUpdate= 0.1f;

	// Use this for initialization
	void Start () {

		//add all racers into  the List of racers
//		Racers = new List<GameObject>() {GameObject.Find("Player"),GameObject.Find("AI04"),GameObject.Find("AI05"),GameObject.Find("AI06")};    //list of racers

		//list to store the lap each racer is currently on
		racerLapNumber= new int[2];
		racerLapNumber [0] = 1;
		racerLapNumber [1] = 1;
//		racerLapNumber [2] = 1;
//		racerLapNumber [3] = 1;


		//list to store the checkpoint of each lap each racer is currently on
		racerCheckpointNumber= new int[2];
		racerCheckpointNumber [0] = 0;
		racerCheckpointNumber [1] = 0;
//		racerCheckpointNumber [2] = 0;
//		racerCheckpointNumber [3] = 0;
	}

	// Update is called once per frame
	void LateUpdate () {

		if(Time.time>=nextUpdate){
			// Delay the next update (current second+1)	- we do not need to calculate the positioning system every frame
			nextUpdate=Mathf.FloorToInt(Time.time)+1;

			// Update the positioning
			calculatePositioning();
		}
	}

	/// <summary>
	/// Calculates the position of each player in the racer and calls the appropriate methods to update the GUI. A summary of this method is:
	/// 	- find out what the highest lap reached is
	/// 	- check if any other racers are on this lap
	/// 		- if there are no other racers on this lap then place the racer found on the highest lap in the postitioning leaderboard
	/// 	- if there are other racers on the same lap then find the highest checkpoint reached by the racers on that lap
	/// 	- check if any other racers are at this checkpoint
	/// 		- if not then place this racer in the postitioning leaderboard
	/// 	- if there are other racers are the same checkpoint then find who is closest distance to the next checkpoint
	/// 		- place the closest racer to the next checkpoint in the postitioning leaderboard
	/// 		- and then iterate through in terms of closest distance, adding to the positioning leaderboard
	/// 	- Repeat this process until every racer has been positioned
	/// </summary>
	void calculatePositioning() {

		List<int> PositionsIndexs = new List<int>();	//list racers are added to to designate position 

		//duplicate racerCheckpointNumber (which contains the current checkpoint of each racer) so it can be modified
		int[] racerCheckpointNumberPositioning = new int[2];   
		System.Array.Copy (racerCheckpointNumber, racerCheckpointNumberPositioning, 2);

		//duplicate racerLapNumber (array of each racer's lap number) so it can be modified
		int[] racerLapNumberPositioning = new int[2];
		System.Array.Copy (racerLapNumber, racerLapNumberPositioning, 2);


		while (PositionsIndexs.Count != 2) {	//while there are still four racers to be positioned

			racerCheckpointNumberPositioning = new int[2];    									//array to store racers checkpoints
			System.Array.Copy (racerCheckpointNumber, racerCheckpointNumberPositioning, 2);		//reset the array every iteration of the while loop

			int highestLap = racerLapNumberPositioning.Max (); //get the highest lap any racer has reached

			List<int> indexOfRacerLap = new List<int> ();		//create list to store the laps each car has reached, should they have also reached the max lap
			indexOfRacerLap.Add (racerLapNumberPositioning.ToList ().IndexOf (racerLapNumberPositioning.Max ()));    //add to list the index of highest lap

			for (int i = 0; i < racerLapNumberPositioning.Length; i++) {    //loop to check if other racers are at the same lap

				if (racerLapNumberPositioning [i].Equals (highestLap)) {    //for each racer if the lap is the same as the highest reached

					//and ensure it is not the same racer we previously found with the hiest lap
					if (i != indexOfRacerLap [0]) {          //if they are at the same lap
						indexOfRacerLap.Add (i);             //add their racer index to list
					}
				}
			}

			if (indexOfRacerLap.Count == 1) {                 						//if only one racer is at the highest lap reached
				PositionsIndexs.Add (indexOfRacerLap [0]);            				//they are in first place (out of remaining racers) so add to the positioning list (no need to further check their checkpoint reached)
				racerCheckpointNumberPositioning [indexOfRacerLap [0]] = -1000;     //change their lap to a low number to stop from being compared in further searches
				racerLapNumberPositioning [indexOfRacerLap [0]] = -1000;

			} else {    //else multiple racers are on same lap - so compare checkpoints to see who if furthest ahead

				//GET MAX LAP - CREATE ARRAY OF CHECKPOINTS OF RACERS ON SAME LAP - THEN GET MAX CHECKPOINT OF RACERS ON SAME LAP

				for (int n = 0; n < racerLapNumberPositioning.Length; n++) {    		//for each racer that is not on the highest lap reached
					if (racerLapNumberPositioning [n] != highestLap) {            		
						racerCheckpointNumberPositioning [n] = -1000;                   //set checkpoint to minimum so they are not compared in the next comparision of checkpoints of racers on the highest lap
					}
				}

				int highestCheckpoint = racerCheckpointNumberPositioning.Max (); 		//find the highest checkpoint reached of racers on the highest lap

				List<int> indexOfRacer = new List<int> ();
				indexOfRacer.Add (racerCheckpointNumberPositioning.ToList ().IndexOf (racerCheckpointNumberPositioning.Max ()));    //get index of highest checkpoint (which racer is on the highest checkpoint)

				//for the array containing each checkpoint a car is at
				for (int i = 0; i < racerCheckpointNumberPositioning.Length; i++) {          //check if other racers are at the same checkpoint

					if (racerCheckpointNumberPositioning [i].Equals (highestCheckpoint)) {   //for each racer if the checkpoint is the same as the highest reached

						//and it is not the same racer    - as checking ith car - if i = indexOfRacer then same car
						if (i != indexOfRacer [0]) {         //if they are a different car at the same checkpoint
							indexOfRacer.Add (i);            //add their racer index to list
						}
					}
				}

				if (indexOfRacer.Count == 1) {                     					//if only one racer at max checkpoint
					PositionsIndexs.Add (indexOfRacer [0]);        					//in first place (out of remaining racers) so add to the positioning list
					racerCheckpointNumberPositioning [indexOfRacer [0]] = -1000;    //change their checkpoint to a low number to stop from being compared in further searches
					racerLapNumberPositioning [indexOfRacer [0]] = -1000;           //change their lap to a low number to stop from being compared in further searches

				} else { //there is more than one racer at the same checkpoint
					List<float> DistanceToNodes = new List<float> ();    //init a list to store distance to the checkpoints

					//compare distance of racers to node
					for (int i = 0; i < indexOfRacer.Count; i++) {     //for each racer
						DistanceToNodes.Add (getDistanceToNode (Racers [indexOfRacer [i]])); //to the list of distances add the distance to the next node of each racer
					}

					//for each distance
					for (int j = 0; j < DistanceToNodes.Count; j++) {
						PositionsIndexs.Add (indexOfRacer [DistanceToNodes.ToList ().IndexOf (DistanceToNodes.Min ())]);    //store closest to node in positioning list
						DistanceToNodes [DistanceToNodes.ToList ().IndexOf (DistanceToNodes.Min ())] = 1000;            	//make value high it won't be selected as min again
						racerCheckpointNumberPositioning [indexOfRacer [j]] = -1000;   										//stop from being compared in next loop
						racerLapNumberPositioning [indexOfRacer [j]] = -1000;
					}

				}

			}

			//print (System.DateTime.Now.ToLongTimeString() + " " + PositionsIndexs [0] + " " + PositionsIndexs [1] + " " + PositionsIndexs [2] + " " + PositionsIndexs [3]);

			//for each car now positioned - update the positioning system GUI
			for (int i = 0; i < PositionsIndexs.Count; i++) {

				int indexx = PositionsIndexs.FindIndex (x => x == i);

				UpdateGUISingleRacer (i, indexx);
			}

		}	//repeat this while loop until every racer has been positioned.
	}

	/// <summary>
	/// Updates the GUI positioning image labelling of a single racer, and if required the text labelling where the player is.
	/// </summary>
	/// <param name="racerNumber">Racer number.</param>
	/// <param name="racePosition">Race position.</param>
	void UpdateGUISingleRacer(int racerNumber, int racePosition) {


		//if the player - then update the positioning text - e.g. 1st, 2nd, 3rd etc.
		if (racerNumber == 0) {
			if (racePosition == 0) {
				player01Position.text = "1st";
				player02Position.text = "2nd";
			} else if (racePosition == 1) {
				player01Position.text = "2nd";
				player02Position.text = "1st";
			} 
//			else if (racePosition == 2) {
//				playerPosition.text = "3rd";
//			} else if (racePosition == 3) {
//				playerPosition.text = "4th";
//			}
		}

		//using the position of the racer passed in, update the leaderboard of images 
		switch (racePosition) {
		case 0:
			//firstPosition.color = racerColour;
//			firstPosition.sprite = carSprites[racerNumber];
			break;
		case 1:
			//secondPosition.color = racerColour;
//			secondPosition.sprite = carSprites[racerNumber];

			break;
//		case 2:
//			//thirdPosition.color = racerColour;
//			thirdPosition.sprite = carSprites[racerNumber];
//
//			break;
//		case 3:
//			//fourthPosition.color = racerColour;
//			fourthPosition.sprite = carSprites[racerNumber];
//			break;
		}

	}

	/// <summary>
	/// Updates the GUI text showing the position of the player.
	/// </summary>
	void UpdateGUIPlayerPosition(int racerNumber, int racePosition) {

		if (racerNumber == 0) {
			if (racePosition - 1 == 0) {
				player01Position.text = "1st";
				player02Position.text = "2nd";
			} else if (racePosition - 1 == 1) {
				player01Position.text = "2nd";
				player02Position.text = "1st";
			} 
//				else if (racePosition - 1 == 2) {
//				playerPosition.text = "3rd";
//			} else if (racePosition - 1 == 3) {
//				playerPosition.text = "4th";
//			}
		}
	}

	/// <summary>
	/// Updates the array containing each racer's checkpoint number.
	/// </summary>
	public void updateRacerCheckpoint(string Racer, int checkpointNumber) {
		switch(Racer)
		{
		case "Player01":
			racerCheckpointNumber [0] = checkpointNumber;
			break;
		case "Player02":
			racerCheckpointNumber [1] = checkpointNumber;
			break;
//		case "AI05":
//			racerCheckpointNumber [2] = checkpointNumber;
//			break;
//		case "AI06":
//			racerCheckpointNumber [3] = checkpointNumber;
//			break;
		}
	}

	/// <summary>
	/// Updates the array containing each racer's lap number.
	/// </summary>
	public void updateRacerLap(string racerName) {
		switch(racerName)
		{
		case "Player01":
			racerLapNumber [0] = racerLapNumber [0]+1;
			break;
		case "Player02":
			racerLapNumber [1] = racerLapNumber [1]+1;
			break;
//		case "AI05":
//			racerLapNumber [2] = racerLapNumber [2]+1;
//			break;
//		case "AI06":
//			racerLapNumber [3] = racerLapNumber [3]+1;
//			break;
		}
	}

	/// <summary>
	/// Returns a float containing the distance between the racer and the next node it is heading towards.
	/// </summary>
	float getDistanceToNode(GameObject Racer) {

		int nodeNum = 0;

		//get the current checkpoint number - differently collected dependent on whether it is AI or player
//		if (Racer.tag == "Player") {
		nodeNum = Racers[0].GetComponent<PlayerMovement> ().GetCurrentCheckpointNo ();
//		} else {
//			nodeNum = (GameObject.Find (Racer.name).GetComponent<AIRacer2> ().getcurrentCheckpointNo ());
//			//Debug.Log (Racer.name + ": checkpointNo" + nodeNum);
//		}

		//if the next node is where the lap starts again then cycle back through
		if (nodeNum < 19) {
			nodeNum++;
		} else {
			nodeNum = 0;
		}

		//find this node so we can get its position
		string node = "Checkpoint" + nodeNum.ToString("00");
		GameObject trackingNode = GameObject.Find (node);

		//Debug.Log ("Next node " + nodeNum + " " + Vector3.Distance (Racer.transform.position, trackingNode.transform.position));

		//return the distance between the racer and the node it is heading towards
		return Vector3.Distance( Racer.transform.position, trackingNode.transform.position);
	}
}