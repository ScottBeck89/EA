using UnityEngine;
using System.Collections;

/*
 * Spikes is a class that represents a type of trap.  It stays idle until the trigger is entered;
 * At which point the damage is done and the spikes are created based off the type of spike it is (sword/spear).
 */
public class Spikes : Spell {
	
	public float spikeDamage = 50.0f;
	public float spikeMaxHealth = 15.0f;
	public float spikeCurHealth = 15.0f;
	public Vector3 offSet;
	public Vector3 spikeSize = new Vector3(1, 1, 1);
	public float scaleSpeed = 0.9f;

	private float matSpeed = 0.4f;

	void Update() {
		float t = Time.time * matSpeed;
		transform.renderer.material.mainTextureOffset = new Vector2(t, t/2);

		if(base.triggered && transform.localScale != spikeSize) {
			transform.localScale = Vector3.Lerp(transform.localScale, spikeSize, scaleSpeed);
		}
	}
	
	public void Reset() {
		radius = 0.75f;
		damage = spikeDamage;
		maxHealth = spikeMaxHealth;
		curHealth = spikeCurHealth;
	}

	public override void trigger() {
		base.triggered = true;
		GameObject.Destroy (gameObject, 2.0f);
	}

}