using UnityEngine;
using System.Collections;

/*
 * SwordSpikes arise up when triggered, then slowly "fall" and dissapear after a short time.
 */
public class SwordSpikes : MonoBehaviour {

	public float lifetime = 8.0f;
	public float fallRate = 0.05f;
	
	private float delayToFall = 1.5f;

	void Start () {
		GameObject.Destroy(gameObject, lifetime);
		StartCoroutine (Descale());
	}

	IEnumerator Descale () {
		yield return new WaitForSeconds(delayToFall);
		while (transform.localScale != new Vector3(1, 0, 1)) {
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 0, 1), fallRate);
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
}
