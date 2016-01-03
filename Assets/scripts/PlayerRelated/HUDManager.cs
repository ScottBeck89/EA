using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour {

	public Text playerHealth;

	public PlayerManager playerManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		playerHealth.text = playerManager.curHealth + " / " + playerManager.maxHealth;
	
	}
}
