using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	public float maxHealth = 100;
	public float curHealth;

	public GameObject deathEffect;

	void Start () {
		curHealth = maxHealth;
	}

	void Update () {}
	
	public void changeEnemyHealth(HEALTH_CHANGE change, float amount) { 
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
	
	private void dead() {
		print ("Enemy " + transform.name + " dead");
		triggerDeath();
	}

	private void triggerDeath() {
		if(deathEffect)
			GameObject.Instantiate (deathEffect, transform.position, Quaternion.identity);
		GameObject.Destroy(gameObject, 0.0f);
	}
}
