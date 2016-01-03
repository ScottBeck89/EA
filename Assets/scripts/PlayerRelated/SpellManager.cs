using UnityEngine;
using System.Collections;

public class SpellManager : MonoBehaviour {
	
	//List of spells
	public GameObject trapSpell;
	public GameObject redSpell;
	public GameObject blueSpell;
	public GameObject greenSpell;

	//Spell Costs
	public int redSpellCost = 25;
	public int blueSpellCost = 25;
	public int greenSpellCost = 25;
	public int trapSpellCost = 25;

	//table of spell costs
	private Hashtable spellCost;
	private float playerRadius = 0.75f;

	void Start() {
		setUpSpellCost();
	}

	//cast the spell at/towards the casting point
	public void cast(SPELLS spellToCast, Vector3 castPoint) {
		switch(spellToCast) {
		case SPELLS.red:
			if(redSpell.name == "Lava")
				Instantiate (redSpell, castPoint + redSpell.GetComponent<Lava>().offSet, Quaternion.identity);
			break;
		case SPELLS.blue:
			if(blueSpell.name == "HealthPool")
				Instantiate (blueSpell, castPoint + blueSpell.GetComponent<HealthPool>().offSet, Quaternion.identity);
			break;
		case SPELLS.green:
			if(greenSpell.name == "ClearPath") {
				GameObject cp = Instantiate (greenSpell, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
				cp.GetComponent<ClearPath>().setTravelPoint(castPoint);
			}
			break;
		case SPELLS.trap:
			if(trapSpell.name == "Mine")
				Instantiate (trapSpell, castPoint + trapSpell.GetComponent<Mine>().offSet, Quaternion.identity);
			if(trapSpell.name == "Spikes")
				Instantiate (trapSpell, castPoint + trapSpell.GetComponent<Spikes>().offSet, Quaternion.identity);
			break;
		default:
			print ("ERROR:  No spell was selected to cast.");
			break;
		}
	}


	//Get the minimum distance the spell must be cast away from the player in order to prevent self-mutilation
	public float getSpellMinDistance(SPELLS s) {
		switch(s){
		case SPELLS.red:
			if(redSpell.name == "Lava")
				return redSpell.GetComponent<Lava>().radius + playerRadius;
			return 0.0f;
		case SPELLS.blue:
			if(blueSpell.name == "HealthPool")
				return blueSpell.GetComponent<HealthPool>().radius;
			return 0.0f;
		case SPELLS.green:
			if(greenSpell.name == "ClearPath")
				return greenSpell.GetComponent<ClearPath>().radius;
			return 0.0f;
		case SPELLS.trap:
			if(trapSpell.name == "Mine")
				return trapSpell.GetComponent<Mine>().radius + playerRadius;
			if(trapSpell.name == "Spikes")
				return trapSpell.GetComponent<Spikes>().radius + playerRadius;
			return 0.0f;
		default:
			return 0.0f;
		}
	}

	//get the spell's max distance it can be cast away from the player
	public float getMaxCastDistance(SPELLS s) {
		switch(s){
		case SPELLS.red:
			if(redSpell.name == "Lava")
				return redSpell.GetComponent<Lava>().maxCastDistance;
			return 0.0f;
		case SPELLS.blue:
			if(blueSpell.name == "HealthPool")
				return blueSpell.GetComponent<HealthPool>().maxCastDistance;
			return 0.0f;
		case SPELLS.green:
			if(greenSpell.name == "ClearPath")
				return greenSpell.GetComponent<ClearPath>().maxCastDistance;
			return 0.0f;
		case SPELLS.trap:
			if(trapSpell.name == "Mine")
				return trapSpell.GetComponent<Mine>().maxCastDistance;
			if(trapSpell.name == "Spikes")
				return trapSpell.GetComponent<Spikes>().maxCastDistance;
			return 0.0f;
		default:
			return 0.0f;
		}
	}

	//return whether or not the spell travels.  It is needed if the spell needs to be placed on the floor
	public bool doesSpellTravel(SPELLS s) {
		switch(s){
		case SPELLS.red:
			if(redSpell.name == "Lava")
				return false;
			return false;
		case SPELLS.blue:
			if(blueSpell.name == "HealthPool")
				return false;
			return false;
		case SPELLS.green:
			if(greenSpell.name == "ClearPath")
				return true;
			return false;
		case SPELLS.trap:
			return false;
		default:
			return false;
		}
	}

	//return the selected spell
	public GameObject getSelectedSpell(SPELLS color) {
		switch(color) {
		case SPELLS.red:
			return redSpell;
		case SPELLS.blue:
			return blueSpell;
		case SPELLS.green:
			return greenSpell;
		case SPELLS.trap:
			return trapSpell;
		default:
			return trapSpell;
		}
	}

	public int getSpellCost(SPELLS s) {
		return (int)spellCost[s];
	}
	//set up initial spell costs
	private void setUpSpellCost() {
		spellCost = new Hashtable();
		spellCost.Add (SPELLS.red, redSpellCost);
		spellCost.Add (SPELLS.blue, blueSpellCost);
		spellCost.Add (SPELLS.green, greenSpellCost);
		spellCost.Add (SPELLS.trap, trapSpellCost);
	}

	//set up a specific spell's cost
	public void setUpSpellCost(SPELLS c, int cost) {
		if(spellCost[c] != null)
			spellCost.Add (c, cost);
		else {
			spellCost.Remove (c);
			spellCost.Add (c, cost);
		}
	}
}

//an enum to represent the possible spells
public enum SPELLS {
	red,
	blue,
	green,
	trap
}
