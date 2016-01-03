using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public GameObject startingPoint;

	public GameObject centerOfGravity;

	public float speed = 1.0f;

	public Vector3 orbitDirection = Vector3.up;

	public bool chaos = false;

	private bool stableOrbit = false;

	private Vector3 startPos;
	private Vector3 endPos;

	private float lerpTime = 1f;
	private float currentLerpTime = 0.0f;

	void Start()
	{
		startPos = transform.position;
	}

	public void SetStartPosition(GameObject newPosition)
	{
		currentLerpTime = 0.0f;
		stableOrbit = false;
		startingPoint = newPosition;
	}
	
	void Update ()
	{
		if ( stableOrbit )
		{
			Vector3 center = centerOfGravity.transform.position;

			if ( chaos)
			{
				StartCoroutine( changeDirection ());

			}

			transform.RotateAround(center, orbitDirection, 20 * Time.deltaTime * speed);
		}
		else
		{
			endPos = startingPoint.transform.position;
			currentLerpTime += Time.deltaTime * .3f;
			if ( currentLerpTime > lerpTime )
			{
				currentLerpTime = lerpTime;
				stableOrbit = true;
			}

			float perc = currentLerpTime / lerpTime;
			perc = perc * perc * perc * ( perc * (6f * perc - 15f)  + 10f );
			transform.position = Vector3.Lerp(startPos, endPos, perc);
		}
	}

	private IEnumerator changeDirection()
	{
		chaos = false;
		float x = Random.value * Time.deltaTime;
		float y = Random.value * Time.deltaTime;
		float z = Random.value * Time.deltaTime;

		orbitDirection = new Vector3( Random.value, Random.value, Random.value );

		while(stableOrbit)
		{
			if ( orbitDirection.magnitude > 2  )
			{
				x *= -1;
				y *= -1;
				z *= -1;
			}

			orbitDirection = new Vector3( orbitDirection.x + x, orbitDirection.y + y, orbitDirection.z + z);

			yield return new WaitForSeconds(.3f);
		}
	}
}
