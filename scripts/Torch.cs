using UnityEngine;
using System.Collections;

/*
 * Torch holds the basic information and some necessary equipment methods
 * for all torches controlled by the player.
 */
public class Torch : MonoBehaviour {


	public GameObject myLight;
	public Vector3 lightOffsetEquipped;				//Sets the light's position when equipped
	public Vector3 lightOffsetUnequipped;			//Sets the light's position when unequipped
	public float pickupDistance = 1.0f;				//How close the player must be to pick up the torch
	public GameObject myParent;
	public float brightnessEquipped = 3.0f;			//Brightness of the light when equipped
	public float brightnessUnEquipped = 5.0f;		//Brightness of the light when unequipped

	//declare reEquipping variables
	private Vector3 myEquipPosition;
	private Vector3 myEquipEuler;
	private Vector3 myEquipScale;

	private bool isEquipped = false;
	private GameObject player;
	private SphereCollider myCollider;				//Necessary for future needs

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if(!isEquipped && (Vector3.Distance (transform.position, player.transform.position) <= pickupDistance)) {
			reEquip ();
		}

		
	}

	//Set the initial properties for when the torch needs to be equipped
	public void equip(Vector3 localPosition, Vector3 localEuler, GameObject parent, bool firstEquip) {
		if(!isEquipped) {
			myCollider = transform.GetComponent <SphereCollider>();
			myEquipPosition = localPosition;
			myEquipEuler = localEuler;
			myParent = parent;
			float x = transform.localScale.x,
			y = transform.localScale.y,
			z = transform.localScale.z;
			myEquipScale = new Vector3(x, y, z);


			myLight.transform.localPosition = lightOffsetEquipped;
			myLight.light.intensity = brightnessEquipped;
			transform.parent = myParent.transform;
			transform.localPosition = myEquipPosition;
			transform.localEulerAngles = myEquipEuler;
			isEquipped = true;
			myCollider.enabled = false;
		}
	}

	//Set its properties back to equipped state
	public void reEquip() {
		
		if(!isEquipped) {
			myLight.transform.localPosition = lightOffsetEquipped;
			myLight.light.intensity = brightnessEquipped;
			transform.parent = myParent.transform;
			transform.localPosition = myEquipPosition;
			transform.localEulerAngles = myEquipEuler;
			transform.localScale = myEquipScale;
			isEquipped = true;
			myCollider.enabled = false;
		}
	}

	//Set properties for the real world
	public void unEquip(Vector3 localScale, Vector3 position, Vector3 localEuler) {
		myLight.transform.localPosition = lightOffsetUnequipped;
		myLight.light.intensity = brightnessUnEquipped;
		transform.parent = null;
		transform.position = position;
		transform.localScale = localScale;
		transform.localEulerAngles = localEuler;
		isEquipped = false;
		myCollider.enabled = true;
	}
}
