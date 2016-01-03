using UnityEngine;
using System.Collections;

/*
 * Button is a simple class that will create a buttonEffect when pressed 
 * once and only once.  It also activates the super's startEffect, which is
 * meant to be the main effect of pressing the button.
 */
public class Button : Triggerable {

	public GameObject buttonPressedEffect;

	public bool buttonPressed = false;
	
	protected override void OnTriggerEnter(Collider collider) {
		if(!buttonPressed) {
			buttonPressed = true;
			startEffect ();
		}
	}

	protected override void startEffect() {
		print ("button pressed");
		base.startEffect ();
		if(buttonPressedEffect != null)
			Instantiate(buttonPressedEffect, transform.position, Quaternion.identity);
	}
}
