using UnityEngine;
using System.Collections;

public class WallExpand : Triggerable {

	public int numberOfTriggers = 1;
	public GameObject wallOne;
	public GameObject wallTwo;

	public int numTriggered = 0;

	protected override void startEffect() {
		numTriggered++;
		print ("in the start");
		if(numTriggered >= numberOfTriggers) {
			openWall();
		}
	}

	private void openWall() {
		wallOne.SetActive (true);
		wallTwo.SetActive (true);
		gameObject.SetActive (false);
	}
}
