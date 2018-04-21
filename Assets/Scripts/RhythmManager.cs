using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public GameObject aBeat;
    public GameObject bBeat;
    public GameObject xBeat;
    public GameObject yBeat;

    public Stack<GameObject> aBeats;
    private Stack<GameObject> bBeats;
    private Stack<GameObject> xBeats;
    private Stack<GameObject> yBeats;

    public GameObject targetA;
    public GameObject targetB;
    public GameObject targetX;
    public GameObject targetY;

    public float bpm;
    private float lastTime, deltaTime, timer;

    

    // Use this for initialization
    void Start()
    {
        lastTime = 0f;
        deltaTime = 0f;
        timer = 0f;
        aBeats = new Stack<GameObject>();
        bBeats = new Stack<GameObject>();
        xBeats = new Stack<GameObject>();
        yBeats = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        int rand = Random.Range(0, 4);
        deltaTime = GetComponent<AudioSource>().time - lastTime;
        timer += deltaTime;

        if (timer >= (60f / bpm))
        {
            //Create the note
            int lane = Random.Range(0, 4);
            Debug.Log(lane);
            switch (lane)
            {
                case 0:
                    GameObject beatA = ((GameObject)Instantiate(aBeat, targetA.transform.position + Vector3.up * 10, Quaternion.identity));
                    beatA.transform.SetParent(this.transform.GetChild(1), false);
                    beatA.GetComponent<RhythmBeat>().Setup(targetA.transform.localPosition, 5, aBeats);
                    aBeats.Push(beatA);
                    break;
               case 1:
                    GameObject beatB = ((GameObject)Instantiate(bBeat, targetB.transform.position + Vector3.up * 10, Quaternion.identity));
                    beatB.transform.SetParent(this.transform.GetChild(1), false);
                    beatB.GetComponent<RhythmBeat>().Setup(targetB.transform.localPosition, 5, bBeats);
                    bBeats.Push(beatB);
                    break;
                case 2:
                    GameObject beatX = ((GameObject)Instantiate(xBeat, targetX.transform.position + Vector3.up * 10, Quaternion.identity));
                    beatX.transform.SetParent(this.transform.GetChild(1), false);
                    beatX.GetComponent<RhythmBeat>().Setup(targetX.transform.localPosition, 5, xBeats);
                    xBeats.Push(beatX);
                    break;
                case 3:
                    GameObject beatY = ((GameObject)Instantiate(yBeat, targetY.transform.position + Vector3.up * 10, Quaternion.identity));
                    beatY.transform.SetParent(this.transform.GetChild(1), false);
                    beatY.GetComponent<RhythmBeat>().Setup(targetY.transform.localPosition, 5, yBeats);
                    yBeats.Push(beatY);
                    break;
            }
           
            timer -= (60f / bpm);           
        }
                
        lastTime = GetComponent<AudioSource>().time;
    }
}
