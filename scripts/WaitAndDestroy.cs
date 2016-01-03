using UnityEngine;
using System.Collections;

public class WaitAndDestroy : MonoBehaviour {

	public float lifetime = 5.0f;

	void Start() {
		GameObject.Destroy (gameObject, lifetime);
	}
}
