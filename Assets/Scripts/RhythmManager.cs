using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	public bool GameStarted {
		set { gameStarted = value; }
	}

	public XboxController controllerNumber;


	private SpriteRenderer aButtonSpriteRenderer;
	private SpriteRenderer bButtonSpriteRenderer;
	private SpriteRenderer xButtonSpriteRenderer;
	private SpriteRenderer yButtonSpriteRenderer;

	public Sprite badSprite;
	public Sprite goodSprite;
	public Sprite okaySprite;
	public Sprite perfectSprite;
	public Sprite missSprite;

	private float aButtonLabelTimer = 0;
	private float bButtonLabelTimer = 0;
	private float xButtonLabelTimer = 0;
	private float yButtonLabelTimer = 0;


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

		aButtonSpriteRenderer = targetA.transform.GetChild ((targetA.transform.childCount-1)).GetComponent<SpriteRenderer>();
		bButtonSpriteRenderer = targetB.transform.GetChild ((targetB.transform.childCount-1)).GetComponent<SpriteRenderer>();
		xButtonSpriteRenderer = targetX.transform.GetChild ((targetX.transform.childCount-1)).GetComponent<SpriteRenderer>();
		yButtonSpriteRenderer = targetY.transform.GetChild ((targetY.transform.childCount-1)).GetComponent<SpriteRenderer>();


		CallInputListeners ();	//starts listening for button presses
    }

	private IEnumerator CheckButtonLabelTimer(SpriteRenderer buttonSpriteRenderer, float labelCountdownTimer) {
		while (labelCountdownTimer > 0) {
			labelCountdownTimer -= Time.deltaTime;
			if (labelCountdownTimer < 0) {
				buttonSpriteRenderer.enabled = false;
			}

			yield return 0;
		}
	}



	void CallInputListeners() {

		StartCoroutine(ControllerButtonListenter(XboxCtrlrInput.XboxButton.A, aBeats, aButtonSpriteRenderer, aButtonLabelTimer));
		StartCoroutine(ControllerButtonListenter(XboxCtrlrInput.XboxButton.B, bBeats, bButtonSpriteRenderer, bButtonLabelTimer));
		StartCoroutine(ControllerButtonListenter(XboxCtrlrInput.XboxButton.X, xBeats, xButtonSpriteRenderer, xButtonLabelTimer));
		StartCoroutine(ControllerButtonListenter(XboxCtrlrInput.XboxButton.Y, yBeats, yButtonSpriteRenderer, yButtonLabelTimer));
	}

	IEnumerator ControllerButtonListenter(XboxCtrlrInput.XboxButton button, Queue<GameObject> beats, SpriteRenderer buttonSpriteRenderer, float labelCountdownTimer) {	//forever repeat the while loop
		while (true) {
			if (XboxCtrlrInput.XCI.GetButtonDown (button, controllerNumber)){				//when A button is pressed
				if (beats.Count > 0) {														//check to see if there are any A buttons on the stack
					beats.Peek ().GetComponent<RhythmBeat>().CalculateBeatScore();			//calculate the score for the beat
					float score = beats.Peek ().GetComponent<RhythmBeat>().Score;			//get the score for the beat
					itemPowerBar.AddPower (score);		//add score to the power bar		//add the score for the beat
					beats.Peek ().GetComponent<RhythmBeat> ().DestroyBeat (true);			//then destroy the beat

					labelCountdownTimer = .5f;												//time until label disappears
					buttonSpriteRenderer.enabled = true;									//reenable the sprite renderer
					StartCoroutine(CheckButtonLabelTimer(buttonSpriteRenderer, labelCountdownTimer));

					if (score == 1) {
						buttonSpriteRenderer.sprite = perfectSprite;
					} else if (score == .65f) {
						buttonSpriteRenderer.sprite = goodSprite;
					} else if (score == .4f || score == .25f) {
						buttonSpriteRenderer.sprite = okaySprite;
					} else if (score == .1f) {
						buttonSpriteRenderer.sprite = badSprite;
					} else if (score == -2f) {
						buttonSpriteRenderer.sprite = missSprite;
					}
				} 
				else if (gameStarted) {														//else there are no beats in stack - but the game has started
					itemPowerBar.RemovePower (2f);	//remove score from the power bar
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
		deltaTime = GetComponent<AudioSource> ().time - lastTime;

		if (gameStarted) {
			timer += deltaTime;

			if (timer >= (60f / bpm)) {
				CreateNewNote ();  
				timer -= (60f / bpm);           
			}
		}
		lastTime = GetComponent<AudioSource> ().time;

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
