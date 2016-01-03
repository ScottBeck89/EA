using UnityEngine;
using System.Collections;

/*
 * LavaGround will deal immediate damage to the player on entering,
 * followed by damage over time and a speed reduction.
 */
public class LavaGround : MonoBehaviour {

	public int initDamage = 5;
	public int dps = 2;
	[Range(0,5)]
	public float speedModPercent;

	private InputManager INPUT_MANAGER;
	private PlayerManager PLAYER_MANAGER;
	protected EnemyManager ENEMY_MANAGER;

	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		INPUT_MANAGER = player.GetComponent<InputManager>();
		PLAYER_MANAGER = player.GetComponent<PlayerManager>();
	}
	
	void OnTriggerEnter(Collider collider) {
		string tag = collider.transform.tag;
		
		if(tag == "Player") {
			print ("enter lava");
			PLAYER_MANAGER.changePlayerHealth (HEALTH_CHANGE.subtract, initDamage);
			StartCoroutine("damagePlayer");
			INPUT_MANAGER.speedPercentage -= speedModPercent;
		} else if(tag == "Enemy") {
			ENEMY_MANAGER = collider.transform.GetComponent<EnemyManager>();
			StartCoroutine ("damageEnemy", ENEMY_MANAGER);
		}
	}
	
	void OnTriggerStay(Collider collider) {}
	
	void OnTriggerExit(Collider collider) {
		string tag = collider.transform.tag;

		if(tag == "Player") {
			print ("exit lava");
			StopCoroutine ("damagePlayer");
			INPUT_MANAGER.speedPercentage += speedModPercent;
		} else if(tag == "Enemy") {

		}
	}

	IEnumerator damageEnemy(EnemyManager EM) {
		do {
			EM.changeEnemyHealth (HEALTH_CHANGE.subtract, 1);
			yield return new WaitForSeconds(1/(float)dps);
		} while(EM.curHealth >= 0);
	}

	IEnumerator damagePlayer() {
		do {
			PLAYER_MANAGER.changePlayerHealth (HEALTH_CHANGE.subtract, 1);
			yield return new WaitForSeconds(1/(float)dps);
		} while(PLAYER_MANAGER.curHealth >= 0);
	}
}
