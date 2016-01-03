using UnityEngine;
using System.Collections;

public class CreateNewCheckpoint : Triggerable {

	protected override void OnTriggerEnter(Collider collider) {
		startEffect ();
	}

	protected override void startEffect() {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		PlayerManager PM = player.GetComponent<PlayerManager>();
		PM.setCheckpointPosition(player.transform.position);

		GameObject.Destroy(gameObject, 0.0f);
	}
}
