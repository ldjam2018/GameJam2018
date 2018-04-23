using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartCountDown : MonoBehaviour {

	public Text countdownText;
	private int countdownVal = 3;

	public GameObject player01;
	public GameObject player02;

	public Image logo;

	public Sprite logoGreen;
	public Sprite logoRed;
	public Sprite logoYellow;
	public Sprite logoOrange;

	// Use this for initialization
	void Start () {



		Invoke ("BeginGameCountDown", 3.2f);

		StartCoroutine (DisplayLogo ());
	}

	private IEnumerator DisplayLogo() {
		float waitTime = 0.3947368421f;
		float endWaitTime = 3.2f - (waitTime * 4);

		yield return new WaitForSeconds (waitTime);
		logo.GetComponent<Image> ().enabled = true;
		logo.GetComponent<Image> ().sprite = logoRed;
		yield return new WaitForSeconds (waitTime);
		logo.GetComponent<Image> ().sprite = logoOrange;
		yield return new WaitForSeconds (waitTime);
		logo.GetComponent<Image> ().sprite = logoYellow;
		yield return new WaitForSeconds (waitTime);
		logo.GetComponent<Image> ().sprite = logoGreen;
		yield return new WaitForSeconds (waitTime);
		logo.GetComponent<Image> ().sprite = logoRed;
		yield return new WaitForSeconds (waitTime);
		logo.GetComponent<Image> ().sprite = logoOrange;
//		yield return new WaitForSeconds (waitTime);
//		logo.GetComponent<Image> ().sprite = logoYellow;
//		yield return new WaitForSeconds (waitTime);
//		logo.GetComponent<Image> ().sprite = logoGreen;
		yield return new WaitForSeconds (waitTime);
		logo.GetComponent<Image> ().enabled = false;
	}
	
	public void BeginGameCountDown() {
		if (countdownVal > 0) {
			countdownText.text = countdownVal.ToString ();
			countdownVal--;
			Invoke ("BeginGameCountDown", 1f);
		} else if (countdownVal == 0) {	//if countdownVal == 0 then the race has begun
			countdownText.text = "START";	//display the GO text
			countdownVal--;
			//enable vehicle movement
			player01.GetComponent<PlayerMovement> ().setMovementDisabled (false);
			player02.GetComponent<PlayerMovement> ().setMovementDisabled (false);

			player01.transform.GetChild(0).GetChild(1).GetComponent<RhythmManager> ().GameStarted = true;
			player02.transform.GetChild(0).GetChild(1).GetComponent<RhythmManager> ().GameStarted = true;


			Invoke ("BeginGameCountDown", 1f);
		} else if (countdownVal == -1) {
			countdownText.text = "";	//after the counter has run out set it to empty to hide it
		}
	}	
}
