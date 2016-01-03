using UnityEngine;
using System.Collections;



public class comeInView : MonoBehaviour {

	public GameObject target;
	private Transform myTransform;
	public float maxRange = 1.0f;
	private MeshRenderer myMesh;
	public GameObject myLight;
	public GameObject[] forceViewers;

	public GameObject objectThatFocuses;
	private FocusOnPlayer FOCUS_ON_PLAYER;

	// Use this for initialization
	void Start () {
		forceViewers[0] = GameObject.FindGameObjectWithTag("Player");

		GameObject[] torches = GameObject.FindGameObjectsWithTag("torch");

		for(int i = 0; i < torches.Length; i++) {
			forceViewers[i + 1] = torches[i];
		}
		if(!target) {
			target = GameObject.FindGameObjectWithTag("Player");
		}

		FOCUS_ON_PLAYER = objectThatFocuses.GetComponent<FocusOnPlayer>();
		FOCUS_ON_PLAYER.setTarget(target);

		myTransform = transform;
		myMesh = GetComponent<MeshRenderer>();
		if(!myLight) {
			myLight = transform.Find("Point Light").gameObject;
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		int closestFV = 0;
		float closestDistance = Mathf.Infinity;
		bool focus = false;

		for(int i = 0; i < forceViewers.Length; i++) {
			float distance = Vector3.Distance (forceViewers[i].transform.position, myTransform.position);
			if(distance <= maxRange) {
				focus = true;
				if(distance < closestDistance) {
					closestDistance = distance;
					closestFV = i;
				}
			}
		}
		if(focus){
			myMesh.enabled = true;
			myLight.light.enabled = true;
			FOCUS_ON_PLAYER.setTarget (forceViewers[closestFV]);
			FOCUS_ON_PLAYER.setFocus(true);
		}
		else {
			myMesh.enabled = false;
			myLight.light.enabled = false;
			FOCUS_ON_PLAYER.setFocus(false);
		}
	}
}
