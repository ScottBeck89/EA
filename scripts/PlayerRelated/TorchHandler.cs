using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * The TorchHandler will place the torches on the Player's back, have the torch
 * disconnect from the player and move to the real world, and vice-versa
 */
public class TorchHandler : MonoBehaviour {

	//declare basic torches
	public GameObject redTorch;
	public GameObject blueTorch;
	public GameObject greenTorch;
	public GameObject genericLight;
	public GameObject torchHolder;
	public int maxDistance = 10;			//Max distance player can place a torch
	public int maxLightDistance;			//max distance player can cast a light
	public Vector3[] offSets;				//Rotational offsets for each torch when equipped

	public int redDeduction = 75;			//Ignore for incomplete system.
	public int blueDeduction = 75;			//Ignore for incomplete system.
	public int greenDeduction = 75;			//Ignore for incomplete system.
	public int lightCost = 5;				//Ignore for incomplete system.
	
	private int maxTorches = 3;
	private GameObject[] torches;			
	private GameObject[] torchPrefabs;		
	private Hashtable torchMap;
	private PlayerManager PLAYERMANAGER;

	void Start () {
		maxLightDistance = maxDistance + 5;

		torches = new GameObject[maxTorches];
		torchPrefabs = new GameObject[maxTorches];
		torchPrefabs[0] = redTorch;
		torchPrefabs[1] = blueTorch;
		torchPrefabs[2] = greenTorch;

		for (int i = 0; i < maxTorches; i++) {
			torches[i] = Instantiate (torchPrefabs[i], new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			Torch t = torches[i].GetComponent ("Torch") as Torch;
			t.equip (Vector3.zero, offSets[i], torchHolder, true);
		}

		PLAYERMANAGER = GetComponent<PlayerManager>();
	}

	//cast the light "spell"
	public void castLight(Vector3 clickPoint) {
		GameObject l = Instantiate (genericLight, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
		l.GetComponent<LightMove>().setTravelPoint(clickPoint);
		PLAYERMANAGER.changeEnergy (SPELLS.trap, lightCost);
	}

	//Place a torch from InputManager by finding it in the torches list then altering its scale, then unequip it
	public void placeTorch (SPELLS torchColor, Vector3 clickPoint) {

		if(torchColor == SPELLS.trap) {
			print ("ERROR:  Passed in a trap (not a torch)to the torch placer.");
			return;
		}

		//by referencing the prefab we can find exactly which torch we need in the torches list
		GameObject torchToPlace = getTorch (torchColor);
		int torchIndex = getTorchIndex (torchToPlace, torchPrefabs);

		if(torchIndex == -1) {
			print ("ERROR:  Could not find " + torchToPlace.name + " in the prefab list");
			return;
		}
		//set the scale for the real world
		Transform parent = torches[torchIndex].transform.parent;
		float x = torches[torchIndex].transform.localScale.x,
			y = torches[torchIndex].transform.localScale.y,
			z = torches[torchIndex].transform.localScale.z;
		while(parent != null) {
			x *= (1/parent.transform.localScale.x);
			y *= (1/parent.transform.localScale.y);
			z *= (1/parent.transform.localScale.z);
			parent = parent.transform.parent;
		}

		//Unequip the torch
		Torch t = torches[torchIndex].GetComponent ("Torch") as Torch;
		t.unEquip (new Vector3(x, y, z), clickPoint, Vector3.zero);
	}

	//Get the torch prefab
	public GameObject getTorch(SPELLS color) {
		switch(color) {
		case SPELLS.red:
			return redTorch;
		case SPELLS.blue:
			return blueTorch;
		case SPELLS.green:
			return greenTorch;
		case SPELLS.trap:
			return genericLight;
		default:
			return redTorch;
		}
	}

	//Get the index of the torch in an array (should just use a hashmap....)
	public int getTorchIndex(GameObject t, GameObject[] torches) {
		for(int i = 0; i < torches.Length; i++) {
			if(torches[i] == t) {
				return i;
			}
		}
		return -1;
	}

	//Find the distance from the player
	public float distanceFromPlayer(Vector3 clickPoint) {
		return Vector3.Distance (this.transform.position, clickPoint);
	}
}
