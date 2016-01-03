using UnityEngine;
using System.Collections;

/*
 * A simple class to scroll a material across an object
 */
public class MaterialScroll : MonoBehaviour {

	
	public float matSpeedx = 0.4f;
	public float matSpeedy = 0.4f;
	
	void Update () {
		transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Time.time * matSpeedx, Time.time * matSpeedy);
	}
}
