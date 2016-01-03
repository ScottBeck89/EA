using UnityEngine;
using System.Collections;

/*
 * Meant to be a base class for all things triggered.  All triggers begin with some sort of start event.
 */
public class Triggerable : MonoBehaviour {

	public GameObject[] effect;

	protected virtual void startEffect() {
		if(effect.Length > 0) {
			foreach(GameObject g in effect) {
				if(g != null)
					g.GetComponent<Triggerable>().startEffect();
			}
		}
	}

	protected virtual void OnTriggerEnter(Collider collider) {
		startEffect ();
	}

	public void TriggerStartEffect() {
		startEffect ();
	}
}
