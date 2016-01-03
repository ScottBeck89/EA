using UnityEngine;
using System.Collections;
/*
 * LightMove is a spell that is cast by the left click when a "Trap" is selected.
 * It will move in a direction and when it hits the clicked point, it will dampen its velocity
 * and either stop or die shorty after
 */
public class LightMove : MonoBehaviour {

	public float initialIntensity = 4.0f;
	public float finalIntensity = 1.0f;
	public float moveSpeed;
	public float height = 0.5f;
	private Vector3 travelPoint = new Vector3(0,0,0);
	public float lightDecay = 0.05f;

	private float minForgiveness = 0.1f;
	private float dampingFactor = 0.9f;
	private bool reachedTarget = false;
	private bool stop = false;

	void Start() {
		light.intensity = initialIntensity;
		travelPoint.y = height;
		transform.position = new Vector3(transform.position.x, height, transform.position.z);
	}

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

		light.intensity = Mathf.Lerp (light.intensity, finalIntensity, Time.deltaTime * lightDecay);
	}

	public void setTravelPoint(Vector3 tp) {
		travelPoint = tp;
	}

	void OnCollisionEnter(Collision c) {
		stop = true;
	}
}
