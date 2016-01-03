using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

	public float radius = 0.5f;
	public float damage = 25;
	public float maxHealth = 10;
	public float curHealth;
	public float maxCastDistance = 4.0f;
	public GameObject[] startEffects;
	public GameObject[] midEffects;
	public GameObject[] endEffects;

	protected bool triggered = false;
	protected PlayerManager PLAYER_MANAGER;
	protected EnemyManager ENEMY_MANAGER;

	private GameObject[] _startEffects;

	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		PLAYER_MANAGER = player.GetComponent<PlayerManager>();
		
		_startEffects = new GameObject[startEffects.Length];
		int i = 0;
		foreach(GameObject g in startEffects) {
			GameObject t = Instantiate (g, transform.position, Quaternion.identity) as GameObject;
			t.transform.parent = transform.parent;
			_startEffects[i] = t;
			i++;
		}
	}
	
	public float getHealth() {
		return curHealth;
	}
	
	public void changeHealth(HEALTH_CHANGE change, float amount) { 
		if(change == HEALTH_CHANGE.add) {
			curHealth += amount;
			if(curHealth > maxHealth) {
				curHealth = maxHealth;
			}
		} else if(change == HEALTH_CHANGE.subtract) {
			curHealth -= amount;
			if(curHealth <= 0) {
				dead();
			}
		} else if(change == HEALTH_CHANGE.set) {
			curHealth = amount;
			if(curHealth > maxHealth) {
				curHealth = maxHealth;
			}
			if(curHealth <= 0) {
				dead();
			}
		}
	}

	public virtual void trigger() {
		print ("Super is calling destroy");
		GameObject.Destroy (gameObject, 0.0f);
	}

	protected virtual void OnCollisionEnter(Collision col) {
		print (col.transform.name);
	}
	
	protected virtual void OnTriggerEnter(Collider collider) {
		string tag = collider.transform.tag;
		if(tag == "Player" && !triggered) {
			trigger();
			StartCoroutine("MidTransition");
			PLAYER_MANAGER.changePlayerHealth (HEALTH_CHANGE.subtract, damage);
		} else if(tag == "Revealer" && !triggered) {
			trigger();
			StartCoroutine("MidTransition");
		} else if(tag == "Enemy" && !triggered) {
			trigger();
			StartCoroutine("MidTransition");
			ENEMY_MANAGER = collider.transform.GetComponent<EnemyManager>();
			ENEMY_MANAGER.changeEnemyHealth(HEALTH_CHANGE.subtract, damage);
		}
	}

	protected virtual void OnTriggerExit(Collider collider) {}

	void MidTransition() {
		if(_startEffects.Length > 0) {
			foreach(GameObject g in _startEffects) 
				GameObject.Destroy(g, 0.0f);
		}
		if(midEffects.Length > 0) {
			foreach (GameObject g in midEffects)
				GameObject.Instantiate(g, transform.position, Quaternion.identity);
		}
	}
	
	protected virtual void dead() {
		print ("this trap is dead");
		GameObject.Destroy (gameObject, 0.0f);
	}


}
