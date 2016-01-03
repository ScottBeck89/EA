using UnityEngine;
using System.Collections;
/*
 * Mine will spawn an explosion on trigger.
 * It was set up as the idea behind an extremely basic spell.
 */
public class Mine : Spell {
	
	public Vector3 offSet;
	public Vector3 explosionOffset;
	public GameObject explosionPrefab;

	protected override void dead() {
		Instantiate (explosionPrefab, (transform.position + explosionOffset), transform.rotation);
		print ("this mine is dead");
		GameObject.Destroy (gameObject, 0.0f);
	}

	public override void trigger() {
		dead ();
	}

}
