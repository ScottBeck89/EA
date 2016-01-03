using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public float maxHealth = 100;
	public float curHealth;
	public int redEnergyMax = 100;			//Ignore for incomplete system.
	public int blueEnergyMax = 100;			//Ignore for incomplete system.
	public int greenEnergyMax = 100;		//Ignore for incomplete system.
	public int trapEnergyMax = 100;			//Ignore for incomplete system.

	[Range(0,10)]
	public float regenRate = 3.0f;			//Regen rate for energy

	//Declare energy levels
	private int redEnergy = 100;
	private int blueEnergy = 100;
	private int greenEnergy = 100;
	private int trapEnergy = 100;
	private Vector3 checkpointPosition;		//When a death occurs, restart at checkpoint

	void Start () {
		//set spawn point as checkpoint
		checkpointPosition = transform.position;
		curHealth = maxHealth;
		//always update energy
		StartCoroutine ("updateEnergy");
	}

	//Update energy based on the regenRate (ticks/second)
	IEnumerator updateEnergy() {
		do {
			if(redEnergy < redEnergyMax)
				redEnergy++;
			if(blueEnergy < blueEnergyMax)
				blueEnergy++;
			if(greenEnergy < greenEnergyMax)
				greenEnergy++;
			if(trapEnergy < trapEnergyMax)
				trapEnergy++;
			yield return new WaitForSeconds(1/regenRate);
		}while(curHealth > 0);
	}
	
	void Update () {}

	//return player's health
	public float getPlayerHealth() {
		return curHealth;
	}

	//return the current energy of the spell
	public int checkEnergy(SPELLS color) {
		switch(color) {
		case(SPELLS.red):
			return redEnergy;
		case(SPELLS.blue):
			return blueEnergy;
		case(SPELLS.green):
			return greenEnergy;
		case(SPELLS.trap):
			return trapEnergy;
		default:
			return 0;
		}
	}

	//change the current energy of the spell
	public void changeEnergy(SPELLS color, int amount) {
		switch(color) {
		case(SPELLS.red):
			redEnergy += amount;
			break;
		case(SPELLS.blue):
			blueEnergy += amount;
			break;
		case(SPELLS.green):
			greenEnergy += amount;
			break;
		case(SPELLS.trap):
			trapEnergy += amount;
			break;
		default:
			break;
		}
	}

	//Ignore for incomplete system.
	public void setMaxEnergy(SPELLS color, int amount) {
		switch(color) {
		case(SPELLS.red):
			redEnergyMax = amount;
			break;
		case(SPELLS.blue):
			blueEnergyMax = amount;
			break;
		case(SPELLS.green):
			greenEnergyMax = amount;
			break;
		case(SPELLS.trap):
			trapEnergyMax = amount;
			break;
		default:
			break;
		}
	}

	//change the health of the player using the HEALTH_CHANGE enum
	public void changePlayerHealth(HEALTH_CHANGE change, float amount) { 
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

	//The player died, return to the checkpoint and reset his health
	private void dead() {
		print ("You're dead");
		transform.position = checkpointPosition;
		curHealth = maxHealth;
	}

	//Don't touch the enemies, try to reach the goal
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "enemy") {
            transform.position = checkpointPosition;
		}
		if(collision.gameObject.name == "goal") {
            transform.position = checkpointPosition;
		}
	}

	//called when hitting a checkpoint zone.
	public void setCheckpointPosition(Vector3 pos) {
		checkpointPosition = pos;
	}
}

//Enum used to determine how health can be modified
public enum HEALTH_CHANGE {
	add, 
	subtract,
	set,
	halve,
	dbl
}
