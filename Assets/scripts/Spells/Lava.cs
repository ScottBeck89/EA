using UnityEngine;
using System.Collections;

/*
 * Lava will create a lavaGround object and a start effect.
 */

public class Lava : MonoBehaviour {

	public GameObject startEffect;
	public Vector3 startEffectOffset;
	public GameObject middleEffect;
	public Vector3 middleEffectScaling;
	public GameObject endEffect;
	public Vector3 offSet;
	public float lifetime = 8.0f;
	public float radius = 2.0f;
	public float maxCastDistance = 6.0f;

	public float matScrollSpeed = 0.5f;


	private float timeAlive;
	private GameObject _startEffect;
	private GameObject _middleEffect;

	//Make new SPELL generic to inherit
	void Start () {
		_startEffect = Instantiate (startEffect, transform.localPosition + startEffectOffset, Quaternion.identity) as GameObject;
		_startEffect.transform.parent = transform;
		_middleEffect = Instantiate (middleEffect, transform.localPosition, Quaternion.identity) as GameObject;
		_middleEffect.transform.parent = transform;
		_middleEffect.transform.localScale = new Vector3(0,_middleEffect.transform.localScale.y,0);

		GameObject.Destroy (gameObject, lifetime);
	}

	void Update () {
		if(_middleEffect.transform.localScale.sqrMagnitude < middleEffect.transform.localScale.sqrMagnitude) {
			_middleEffect.transform.localScale += middleEffectScaling;
		}

		float offset = Time.time * matScrollSpeed;
		_middleEffect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset, 0);

	}
}
