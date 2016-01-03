using UnityEngine;
using System.Collections;
/*
 * HealthPool is a spell to heal the player over a short period of time.
 */
public class HealthPool : MonoBehaviour {

	public int hps = 3;
	public Vector3 offSet = new Vector3(0, 0.2f, 0);
	public float radius = 0.0f;
	public float maxCastDistance = 3.0f;
	public float lifetime = 5.0f;
	public float showTime;

	private PlayerManager PLAYER_MANAGER;
	private float timeAlive = 0.0f;

	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		PLAYER_MANAGER = player.GetComponent<PlayerManager>();
		showTime = lifetime + 4;
	}

	void Update () {
		timeAlive += Time.deltaTime;
		if(lifetime <= timeAlive) {
			StopCoroutine("healPlayer");
		}
		if(showTime <= timeAlive) {
			GameObject.Destroy (gameObject, 0.0f);
		}
	}
	
	void OnTriggerEnter(Collider collider) {
		string tag = collider.transform.tag;
		
		if(tag == "Player" && timeAlive <= lifetime) {
			print ("enter healing");
			StartCoroutine("healPlayer");
		}
	}
	
	void OnTriggerStay(Collider collider) {}
	
	void OnTriggerExit(Collider collider) {
		string tag = collider.transform.tag;
		
		if(tag == "Player") {
			print ("exit healing");
			StopCoroutine ("healPlayer");
		}
	}
	
	IEnumerator healPlayer() {
		yield return new WaitForSeconds(0.01f);

		while(PLAYER_MANAGER.curHealth <= PLAYER_MANAGER.maxHealth) {
				PLAYER_MANAGER.changePlayerHealth (HEALTH_CHANGE.add, 1);
			yield return new WaitForSeconds(1/(float)hps);
		} 
	}
}
