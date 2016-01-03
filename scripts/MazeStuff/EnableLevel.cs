using UnityEngine;
using System.Collections;

/*
 * EnableLevel disables certain sections of a level, not the level itself.
 * (Although, since it is one scene, it is one level.)
 */
public class EnableLevel : Triggerable {

	public GameObject levelToEnable;
	public GameObject levelToDisable;
	
	protected override void OnTriggerEnter(Collider collider) {
		if(collider.transform.tag == "Player")
			startEffect ();
	}
	
	protected override void startEffect() {
		levelToEnable.SetActive(true);
		levelToDisable.SetActive(false);
	}
}
