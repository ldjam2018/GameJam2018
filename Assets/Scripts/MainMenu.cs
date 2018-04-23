using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {

	public Image logo;

	public Sprite logoGreen;
	public Sprite logoRed;
	public Sprite logoYellow;
	public Sprite logoOrange;

	public Text loadingText;

	// Use this for initialization
	void Start () {
		StartCoroutine (FlashLogo ());
	}

	private IEnumerator FlashLogo(){

		float waitTime = 1f;

		while (true) {
			yield return new WaitForSeconds (waitTime);
			logo.GetComponent<Image> ().sprite = logoRed;
			yield return new WaitForSeconds (waitTime);
			logo.GetComponent<Image> ().sprite = logoOrange;
			yield return new WaitForSeconds (waitTime);
			logo.GetComponent<Image> ().sprite = logoYellow;
			yield return new WaitForSeconds (waitTime);
			logo.GetComponent<Image> ().sprite = logoGreen;
		}
	}

	// Update is called once per frame
	void Update () {


		if (XCI.GetButton (XboxButton.Start)) {
			loadingText.text = "LOADING...";
			SceneManager.LoadScene("MainScene");
		}
	}
}
