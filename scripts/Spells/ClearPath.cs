using UnityEngine;
using System.Collections;

/*
 * ClearPath is a spell that will trigger deadly traps.
 * Its movement is identical to that of LightMove, but will destory itself
 *  quickly on contact with anything in the environment.
 */
public class ClearPath : Spell {

	public Vector3 offSet;
	public float lifetime = 3.0f;

	public float moveSpeed;
	public float height = 0.5f;

	private Vector3 travelPoint = new Vector3(0,0,0);
	private float minForgiveness = 0.1f;
	private float dampingFactor = 0.95f;
	private bool reachedTarget = false;
	private bool stop = false;

	//FixedUpdate for rigidbody movement
	void FixedUpdate () {
		if(!stop) {
			if(Vector3.Distance (transform.position, travelPoint) <= minForgiveness || reachedTarget) {
				reachedTarget = true;
				rigidbody.AddRelativeForce(-(rigidbody.velocity.normalized * moveSpeed * dampingFactor));
				GameObject.Destroy (gameObject, 1.0f);
			} else {
				Vector3 direction = (Vector3)(travelPoint - transform.position).normalized;
				rigidbody.velocity = Vector3.ClampMagnitude(direction * moveSpeed, moveSpeed);
			}
		} else {
			rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity * 0, 0);
			GameObject.Destroy (gameObject, 0.5f);
		}

		//Lives for no longer than desired.
		GameObject.Destroy (gameObject, lifetime);
	}

	//Sets the destination
	public void setTravelPoint(Vector3 tp) {
		travelPoint = tp;
		travelPoint.y = 0.5f;
	}

	//Override: On contact with anything other than the player (mostly the environment or an enemy) kill it quickly
	protected override void OnCollisionEnter(Collision c) {
		string tag = c.transform.tag;
		
		if(tag != "Player" && tag != "Revealer")
			stop = true;
	}

	//Necessary override
	protected override void OnTriggerEnter(Collider collider) {}
}
