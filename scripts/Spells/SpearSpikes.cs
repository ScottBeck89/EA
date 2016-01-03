using UnityEngine;
using System.Collections;
/*
 * SpearSpikes are a variation of Spikes.  Its main difference is that it will
 * slowly fade out rather than "fall down" after it is triggered.   The trigger effect
 * is also different, creating the spikes OnTriggerEnter.
 */
public class SpearSpikes : MonoBehaviour {

	public float lifetime = 8.0f;
	public float fadeAfter = 1.25f;
	public float fadeRate = 0.98f;

	void Start () {
		GameObject.Destroy(gameObject, lifetime);
		StartCoroutine (fadeOut());
	}
	
	IEnumerator fadeOut () {
		yield return new WaitForSeconds(fadeAfter);
		while (gameObject) {
			for (int i = 0; i < transform.childCount; i++) {
				Transform t = transform.GetChild (i);
				t.renderer.material.color = new Color(t.renderer.material.color.r, t.renderer.material.color.g, t.renderer.material.color.b, t.renderer.material.color.a * fadeRate);
			}
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
}
