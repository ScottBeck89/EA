using UnityEngine;
using System.Collections;

/*
 * MoveToObject is designed to have the player teleport around, mainly through simple goals.
 */
public class MoveToObject : MonoBehaviour {
	
	public GameObject transferObject;
	private GameObject player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void OnTriggerEnter(Collider c) {
		string tag = c.transform.tag;
		if(tag == "Player") {
			print ("Teleporting player " + c.transform.name);
			//set new starting position for player
			player.transform.position = transferObject.transform.position;
		}
	}
}
