using UnityEngine;
using System.Collections;

public class FocusOnPlayer : MonoBehaviour {

	public GameObject target;
	public bool focus = false;

	private Transform go;
	private Transform myTransform;
	public float maxRotationSpeed = 1.0f;

	
	// Use this for initialization
	void Start () {
		if(!target) {
			target = GameObject.FindGameObjectWithTag("Player");
		}
		go = target.transform;
		myTransform = transform;
	}



	public void setTarget(GameObject newTarget) {
		this.target = newTarget;
		this.go = target.transform;
	}

	public void setFocus(bool isFocusing) {
		this.focus = isFocusing;
	}

	// Update is called once per frame
	void Update () {

		if(!focus) {
			return;
		} else {
		
			Vector3 targetDirection = go.position - myTransform.position;
			targetDirection.y = transform.position.y;
			float step = maxRotationSpeed * Time.deltaTime;
			Vector3 newDirection = Vector3.RotateTowards (transform.forward, targetDirection, step, 0.0f);
			Debug.DrawRay(transform.position, newDirection, Color.red);
			transform.rotation = Quaternion.LookRotation (newDirection);
		}

	}
}
