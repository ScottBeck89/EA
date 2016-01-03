using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	public float height = 1.0f;

	public float bounceRate = 1.0f;

	private Vector3 direction = Vector3.up;

	void Update () 
	{

		if ( transform.localPosition.y > height)
		{
			direction = Vector3.down;
		}
		else if (transform.localPosition.y <= 0 )
		{
			direction = Vector3.up;
		}
		transform.localPosition += direction * bounceRate * Time.deltaTime;
	}
}
