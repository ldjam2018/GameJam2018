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

    void Start()
    {
        startPosition = transform.position;
//		Invoke ("ResetScore ()", 1f);
    }

	void ResetScore(){
		score = 0;
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
		if (coll.gameObject.name == "BadHit") {
//			Debug.Log ("Hit bad");
			if (score < .2f && timeElapsed > .1f) {
				score = .2f;
			}
		}
		else if (coll.gameObject.name == "GoodHit") {
			if (score < .5f) {
				score = .5f;
			}
		}
		else if (coll.gameObject.name == "GoodHit") {
			if (score < 1f) {
				score = 1f;
			}
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
		if (coll.gameObject.name == "BadHit") {
//			Debug.Log ("Exiting bad");

			if (score == .2f) {		//if in the final bad - revert back to no score
				score = 0f;
			}
		}
		if (coll.gameObject.name == "GoodHit") {
			if (score <= .5f) {		//if not still in perfect
				score = .2f;		//revert back to bad score
			}
		}
		if (coll.gameObject.name == "PerfectHit") {
			score = .5f;			//revert back to good score
		}
	} 

}



