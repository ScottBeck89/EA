using UnityEngine;
using System.Collections;

public class CollideAndOrbit : MonoBehaviour {

	public GameObject disable;
	public GameObject enableOrbit;

	void OnTriggerEnter(Collider collider)
	{
		if ( collider.tag == "Player")
		{
			disable.SetActive(false);

			enableOrbit.transform.SetParent ( collider.gameObject.transform );
			
			enableOrbit.transform.rotation = Quaternion.identity;

			Orbit orb = enableOrbit.GetComponent<Orbit>();

			Orbiter orbiter = collider.GetComponent<Orbiter>();
			
			orb.SetStartPosition( orbiter.startingPoint );
			orb.centerOfGravity = orbiter.centerOfGravity;
			orb.speed = 1.0f;

			orb.orbitDirection = Vector3.up;
			//orb.transform.localScale = new Vector3(1, 1, 1);

			Destroy(gameObject);
		}
	}
}
