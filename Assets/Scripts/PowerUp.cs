using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public GameObject fireWorks;
	
	void Update () {
		transform.Rotate (0, 60 * Time.deltaTime, 0);
	}

    public void Explode()
    {
//        Instantiate(fireWorks, transform.position, Quaternion.identity);
		StartCoroutine(ShrinkAndDestroy());
    }


	private IEnumerator ShrinkAndDestroy(){

		this.GetComponent<Collider> ().enabled = false;

		float reduceScaleByPerFrame = gameObject.transform.localScale.x / 10;

		while (gameObject.transform.localScale.x > 0.001) {
			transform.localScale = new Vector3 (gameObject.transform.localScale.x - reduceScaleByPerFrame, gameObject.transform.localScale.y - reduceScaleByPerFrame, gameObject.transform.localScale.z - reduceScaleByPerFrame);
			yield return 0;
		}

		Destroy (this.gameObject);
	}
}
