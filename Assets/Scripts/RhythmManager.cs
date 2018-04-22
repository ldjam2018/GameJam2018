using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class RhythmManager : MonoBehaviour
{

	//prefab that is spawned and moves down screen
    public GameObject aBeat;
    public GameObject bBeat;
    public GameObject xBeat;
    public GameObject yBeat;


	//stack of all beats
    private Queue<GameObject> aBeats;
	private Queue<GameObject> bBeats;
	private Queue<GameObject> xBeats;
	private Queue<GameObject> yBeats;

	//objects at bottom of screen
    public GameObject targetA;
    public GameObject targetB;
    public GameObject targetX;
    public GameObject targetY;

    public float bpm;
    private float lastTime, deltaTime, timer;

	private ItemPowerBar itemPowerBar;

	private bool gameStarted = false;

	public XboxController controllerNumber;

    // Use this for initialization
    void Start()
    {
        lastTime = 0f;
        deltaTime = 0f;
        timer = 0f;
        aBeats = new Queue<GameObject>();
		bBeats = new Queue<GameObject>();
		xBeats = new Queue<GameObject>();
		yBeats = new Queue<GameObject>();


		itemPowerBar = transform.parent.GetChild (2).GetComponent<ItemPowerBar> ();

		CallInputListeners ();	//starts listening for button presses
    }


	void CallInputListeners() {

		StartCoroutine(ControllerButtonListenter(XboxCtrlrInput.XboxButton.A, aBeats));
		StartCoroutine(ControllerButtonListenter(XboxCtrlrInput.XboxButton.B, bBeats));
		StartCoroutine(ControllerButtonListenter(XboxCtrlrInput.XboxButton.X, xBeats));
		StartCoroutine(ControllerButtonListenter(XboxCtrlrInput.XboxButton.Y, yBeats));
	}

	IEnumerator ControllerButtonListenter(XboxCtrlrInput.XboxButton button, Queue<GameObject> beats) {	//forever repeat the while loop
		while (true) {
			if (XboxCtrlrInput.XCI.GetButtonDown (button, controllerNumber)){				//when A button is pressed
				if (beats.Count > 0) {															//check to see if there are any A buttons on the stack
					//					float t = CalculateScoreFromBeat (aBeats);									//if there is, calculate the score from it
					float score = beats.Peek ().GetComponent<RhythmBeat>().Score;
					if (score == .2f) {																//if the beat was correctly hit on time
						itemPowerBar.AddPower (score);		//add score to the power bar
						beats.Peek ().GetComponent<RhythmBeat> ().DestroyBeat (true);				//then destroy the beat
						//scale up and red
						//text bad
					} else if (score == .5f) {
						beats.Peek ().GetComponent<RhythmBeat> ().DestroyBeat (true);				//then destroy the beat
						itemPowerBar.AddPower (score);	//remove score from the power bar
						//particle effect for hitting on time
						//mid animation - scale up and yellow
						//text good!
					} else if (score == 1f) {
						beats.Peek ().GetComponent<RhythmBeat> ().DestroyBeat (true);				//then destroy the beat
						itemPowerBar.AddPower (score);	//remove score from the power bar
						//particle effect for hitting on time
						//good animation - scale up and green
						//text Perfect!
					}
				} else if (gameStarted) {													//else there are not points in stack - but the game has started
					itemPowerBar.RemovePower (.15f);	//remove score from the power bar
					//play bad noise
					//bad animation - scale up and red
				} else /*if (!gameStarted)*/{												//else the game has not started
					//call animation that increases scale of targetA						//so no negative repurcussions
					//play a mild noise
				}
			}
			yield return 0;
		}
	}

    // Update is called once per frame
    void Update()
    {
        deltaTime = GetComponent<AudioSource>().time - lastTime;
        timer += deltaTime;

        if (timer >= (60f / bpm))
        {
			CreateNewNote();  
            timer -= (60f / bpm);           
        }
        lastTime = GetComponent<AudioSource>().time;
    }

	private void CreateNewNote() {
		//Create the note
		int lane = Random.Range(0, 5);
		switch (lane)
		{
		case 0:
			GameObject beatA = ((GameObject)Instantiate(aBeat, targetA.transform.position + Vector3.up * 1000, Quaternion.identity));
			beatA.transform.SetParent(this.transform.GetChild(1), false);
			beatA.GetComponent<RhythmBeat>().Setup(targetA, 5, aBeats, itemPowerBar);
			aBeats.Enqueue(beatA);
			break;
		case 1:
			GameObject beatB = ((GameObject)Instantiate(bBeat, targetB.transform.position + Vector3.up * 1000, Quaternion.identity));
			beatB.transform.SetParent(this.transform.GetChild(1), false);
			beatB.GetComponent<RhythmBeat>().Setup(targetB, 5, bBeats, itemPowerBar);
			bBeats.Enqueue(beatB);
			break;
		case 2:
			GameObject beatX = ((GameObject)Instantiate(xBeat, targetX.transform.position + Vector3.up * 1000, Quaternion.identity));
			beatX.transform.SetParent(this.transform.GetChild(1), false);
			beatX.GetComponent<RhythmBeat>().Setup(targetX, 5, xBeats, itemPowerBar);
			xBeats.Enqueue(beatX);
			break;
		case 3:
			GameObject beatY = ((GameObject)Instantiate(yBeat, targetY.transform.position + Vector3.up * 1000, Quaternion.identity));
			beatY.transform.SetParent(this.transform.GetChild(1), false);
			beatY.GetComponent<RhythmBeat>().Setup(targetY, 5, yBeats, itemPowerBar);
			yBeats.Enqueue(beatY);
			break;
		}
	}
}
