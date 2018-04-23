using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBeat : MonoBehaviour
{
	private float timeElapsed;		//timeElapsed = 1 means perfectly on target
	public float TimeElapsed {
		 get {return timeElapsed;}
	}

    public GameObject explosion;
    public Vector3 startPosition;
	public GameObject target;
    public float timeToReachTarget;

	bool destroying = false;

    private Queue<GameObject> stack;
	private bool popped = false;
	private bool completed = false;

	private ItemPowerBar itemPowerBar;
	private float score = 0f;
	public float Score {
		get {return score;}
	}

	bool badTop = false;
	bool goodTop = false;
	bool perfect = false;
	bool goodBottom = false;
	bool badBottom = false;

    void Start()
    {
        startPosition = transform.position;
//		Invoke ("ResetScore ()", 1f);
    }

	void ResetScore(){
		score = 0;
	}

	public void CalculateBeatScore() {
		int numberMatched = 0;

		if (badTop == true) {
			numberMatched++;
		}
		if (goodTop == true) {
			numberMatched++;
		}
		if (perfect == true) {
			numberMatched++;
		}
		if (badBottom == true) {
			numberMatched++;
		}
		if (goodBottom == true) {
			numberMatched++;
		}

		switch (numberMatched) {
		case 5:
			score = 1;
			//set text
			break;
		case 4:
			score = .65f;
			break;
		case 3:
			score = .4f;
			break;
		case 2:
			score = .25f;
			break;
		case 1:
			score = .1f;
			break;
		case 0:
			score = -2f;
			break;
		}
	}

    void Update()
    {
        timeElapsed += Time.deltaTime / timeToReachTarget;
        if (timeElapsed <= 1)
		{	//targetA.transform.position + Vector3.up * 10

			Vector3 y = target.transform.localPosition + Vector3.up * 10;
			transform.localPosition = Vector3.Lerp(new Vector3 (target.transform.localPosition.x, y.y, target.transform.localPosition.z), target.transform.localPosition, timeElapsed);
		} else if (!destroying)
        {
			transform.localPosition = Vector3.Lerp(target.transform.localPosition, target.transform.localPosition + Vector3.down * 10, (timeElapsed - 1));

			if (timeElapsed > 1.1f && popped == false) {
				popped = true;
				stack.Dequeue();

			}

            if (timeElapsed > 1.5f)
            {
                DestroyBeat(false);
				//pass score to power meter

            }
        }
    }

	public void Setup(GameObject destination, float time, Queue<GameObject> stack, ItemPowerBar itemPowerBar)
    {
        timeElapsed = 0;
		startPosition = transform.localPosition;
        timeToReachTarget = time;
        target = destination;
		this.stack = stack; this.itemPowerBar = itemPowerBar;
    }

	public void /*float*/ DestroyBeat(bool completed)
    {
		if (!popped) {
			stack.Dequeue ();
			popped = true;
		}

		this.completed = completed;

		StartCoroutine (ShrinkAndDestroy ());
    }

	private IEnumerator ShrinkAndDestroy() {
		destroying = true;
		float reduceScaleByPerFrame = gameObject.transform.localScale.x/10;

		while (gameObject.transform.localScale.x > 0.01f) {
			transform.localScale = new Vector3 (gameObject.transform.localScale.x - reduceScaleByPerFrame, gameObject.transform.localScale.y - reduceScaleByPerFrame, gameObject.transform.localScale.z - reduceScaleByPerFrame);
			yield return 0;
		}
		GameObject beatExplosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
		beatExplosionInstance.transform.SetParent (this.transform.parent, true);
        Destroy(gameObject);

	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "BadHitTop") {
			badTop = true;
		}
		else if (coll.gameObject.name == "GoodHitTop") {
			goodTop = true;

		}
		else if (coll.gameObject.name == "PerfectHit") {
			perfect = true;
		}
		else if (coll.gameObject.name == "GoodHitBottom") {
			goodBottom = true;
		}
		else if (coll.gameObject.name == "BadHitBottom") {
			badBottom = true;
		}
		else if (coll.gameObject.name == "RemoveFromQueue") {
			if (!popped) {
				popped = true;
				stack.Dequeue ();
				if (!completed) {
					itemPowerBar.RemovePower (.5f);
				}
			}
		}

	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.name == "BadHitTop") {
			badTop = false;
		}
		else if (coll.gameObject.name == "GoodHitTop") {
			goodTop = false;

		}
		else if (coll.gameObject.name == "PerfectHit") {
			perfect = false;
		}
		else if (coll.gameObject.name == "GoodHitBottom") {
			goodBottom = false;
		}
		else if (coll.gameObject.name == "BadHitBottom") {
			badBottom = false;
		}
	} 

}



