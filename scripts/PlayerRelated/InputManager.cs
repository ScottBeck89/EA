using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	private Vector3 input2D;						//A Vector used to simplify the casting of spells, since it is all done in 2D.
	private SpellManager SPELLMANAGER;
	private TorchHandler TORCHHANDLER;
	private PlayerManager PLAYERMANAGER;
	private float minDistanceFromPlayer = 0.75f;	//The minimum distance from the player's center a click needs to be in order to be registered.
	private float dampingFactor = 0.95f;			//A damping factor for movement AFTER the button has been released.
	private Hashtable cdTracker;					//Hashtable for cooldowns. 
	private LayerMask lMask = (1 << 8); 			//SpellColliderLayer to ignore during clicking for casting
	
	public float maxSpeed = 5f;						//Player's max speed
	[Range(0,3)]
	public float speedPercentage = 1.0f;			//The percentage of a player's speed.  e.g. 1 = 100%
	public SPELLS selectedSpell;					//The currently selected spell for casting


	RaycastHit hit = new RaycastHit();
	Ray ray = new Ray();


	void Start() {
		//Default to the trap spell.
		selectedSpell = SPELLS.trap;
	
		TORCHHANDLER = GetComponent<TorchHandler>();
		SPELLMANAGER = GetComponent<SpellManager>();
		PLAYERMANAGER = GetComponent<PlayerManager>();

		//A cooldown references the spell, a countdown until recast, if it has been cast, and its cooldown time.
		cdTracker = new Hashtable();
		cdTracker[SPELLMANAGER.redSpell] = new Cooldown(SPELLMANAGER.redSpell, 1.0f);
		cdTracker[SPELLMANAGER.blueSpell] = new Cooldown(SPELLMANAGER.blueSpell, 1.0f);
		cdTracker[SPELLMANAGER.greenSpell] = new Cooldown(SPELLMANAGER.greenSpell, 1.0f);
		cdTracker[SPELLMANAGER.trapSpell] = new Cooldown(SPELLMANAGER.trapSpell, 1.0f);
		cdTracker[TORCHHANDLER.redTorch] = new Cooldown(TORCHHANDLER.redTorch, 1.0f);
		cdTracker[TORCHHANDLER.blueTorch] = new Cooldown(TORCHHANDLER.blueTorch, 1.0f);
		cdTracker[TORCHHANDLER.greenTorch] = new Cooldown(TORCHHANDLER.greenTorch, 1.0f);
		cdTracker[TORCHHANDLER.genericLight] = new Cooldown(TORCHHANDLER.genericLight, 1.0f);
		
		lMask = ~lMask;
	}

	void Update() {

		//Reference Unity's input manager to select a spell.
		if(Input.GetButtonDown ("QSpell")) {
			selectedSpell = SPELLS.red;
		}
		if(Input.GetButtonDown ("ESpell")) {
			selectedSpell = SPELLS.blue;
		}
		if(Input.GetButton ("ZSpell")) {
			selectedSpell = SPELLS.green;
		}
		if(Input.GetButton ("CSpell")) {
			selectedSpell = SPELLS.trap;
		}

		//Increment the cooldowns.  Probably should be done elsewhere.
		if(cdTracker.Values.Count > 0) {
			foreach(Cooldown g in cdTracker.Values) {
				if(g.isCasting) {
					g.countdown += Time.deltaTime;
				}
				if(g.countdown >= g.cooldownTime) {
					print (g.spell.name + " is off cooldown");
					g.isCasting = false;
					g.countdown = 0.0f;
				}
			}
		}
	}

	//FixedUpdate for Rigidbody calculations
	void FixedUpdate () {

		Vector3 clickPoint;
		//The input for two-directional movement.
		input2D = new Vector3(Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

		//If we have not pressed any of the buttons, slow us down but only if we are still moving.
		if(input2D.magnitude == 0 && rigidbody.velocity.magnitude > 0) {
			rigidbody.AddRelativeForce(-(rigidbody.velocity.normalized * dampingFactor));
		} else {
			//This gives an immediate feedback in movement.  Instant acceleration clamped to maxSpeed.
			rigidbody.velocity = Vector3.ClampMagnitude(input2D * maxSpeed, maxSpeed * speedPercentage);
		}

		//Casting a spell
		if(Input.GetMouseButtonDown (1)) {
			Cooldown s = (Cooldown) cdTracker[SPELLMANAGER.getSelectedSpell(selectedSpell)];

			//Check that we are not casting already and we have enough energy to cast based on the Spell's cost
			if(!s.isCasting && PLAYERMANAGER.checkEnergy(selectedSpell) > SPELLMANAGER.getSpellCost(selectedSpell)){
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				//Ignoring other spells, ensure the point hit is at least the minimum required distance from the player and less than its max cast distance
				if(Physics.Raycast(ray, out hit, Mathf.Infinity, lMask) && 
				   SPELLMANAGER.getSpellMinDistance(selectedSpell) < distanceFromPlayer(hit.point) &&
				   distanceFromPlayer(hit.point) <= SPELLMANAGER.getMaxCastDistance(selectedSpell)) {
					print ("did raycast");

					//We can cast the Spell!
					//Change the energy level based on the Spell's cost.
					PLAYERMANAGER.changeEnergy(selectedSpell, -(SPELLMANAGER.getSpellCost(selectedSpell)));

					//If it is a travelling spell, we don't care what the raycast hits, just get the direction
					if(SPELLMANAGER.doesSpellTravel(selectedSpell)) {
						clickPoint = hit.point;
						SPELLMANAGER.cast(selectedSpell, (Vector3)clickPoint);
						s.isCasting = true;
						print (SPELLMANAGER.getSelectedSpell(selectedSpell).name + " is casting");
					//If it is a trap, it must hit the floor.
					} else if (hit.collider.tag == "floor") {
						clickPoint = hit.point;
						SPELLMANAGER.cast(selectedSpell, (Vector3)clickPoint);
						s.isCasting = true;
						print (SPELLMANAGER.getSelectedSpell(selectedSpell).name + " is casting");
					}
				}
			}
		//Left Clicking (torch and light)
		} else if(Input.GetMouseButtonDown (0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			//Casting a moving light
			if(selectedSpell == SPELLS.trap) {
				Cooldown s = (Cooldown) cdTracker[TORCHHANDLER.getTorch(selectedSpell)];

				//If it is not casting, don't worry about cost
				if(!s.isCasting){
					//Ignoring other spells, ensure the point hit is at least the minimum required distance from the player and less than its max cast distance
					if(Physics.Raycast(ray, out hit, Mathf.Infinity, lMask)  &&
					   minDistanceFromPlayer < distanceFromPlayer(hit.point) &&
					   distanceFromPlayer(hit.point) <= TORCHHANDLER.maxLightDistance) {

						//Cast the light, not worrying about what the raycast hits
						clickPoint = hit.point;
						TORCHHANDLER.castLight((Vector3)clickPoint);
						s.isCasting = true;
						print ("Moving Light is casting");
					}
				}
			//Placing a torch
			}else {
				Cooldown s = ((Cooldown) cdTracker[TORCHHANDLER.getTorch(selectedSpell)]);
				if(!s.isCasting) {

					//Ignoring other spells, ensure the point hit is at least the minimum required distance from the player and less than its max cast distance
					if(Physics.Raycast(ray, out hit, Mathf.Infinity, lMask) && 
					   (minDistanceFromPlayer + 0.5f < distanceFromPlayer(hit.point)) &&
					   distanceFromPlayer(hit.point) <= TORCHHANDLER.maxDistance) {

						//Must click the floor, place the torch (later it will enable it's ability/spell)
						if(hit.collider.tag == "floor") {
							clickPoint = hit.point;
							TORCHHANDLER.placeTorch(selectedSpell, (Vector3)clickPoint);
							s.isCasting = true;
							print (TORCHHANDLER.getTorch(selectedSpell).name + " is casting");
						}
					}
				}
			}

		}
	}

	//Grab the distance from the player
	public float distanceFromPlayer(Vector3 clickPoint) {
		return Vector3.Distance (transform.position, new Vector3(clickPoint.x, transform.position.y, clickPoint.z));
	}

	//Cooldown class used for keeping track of a spell's cooldowns.
	public class Cooldown {
		public GameObject spell;
		public float cooldownTime;
		public bool isCasting = false;
		public float countdown = 0.0f;
		
		public Cooldown(GameObject g, float c) {
			spell = g;
			cooldownTime = c;
		}
	}
}
