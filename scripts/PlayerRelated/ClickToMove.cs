﻿using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {

	public float slideForgiveness = 0.5f;
	public float acceleration;
	private float maxSpeed = 5.0f;
	private bool clickedToMove = false;
	private bool atDestination = false;
	private Vector3 clickPoint;
	private Vector3 my2DPosition;

	RaycastHit hit = new RaycastHit();
	Ray ray = new Ray();
	private Vector3 startingPosition;
	
	void Start() {
		startingPosition = rigidbody.position;
	}
	
	// Update is called once per frame
	void Update () {

		my2DPosition = rigidbody.position;
		my2DPosition.y = 0;

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Input.GetMouseButtonDown (0) && Physics.Raycast(ray, out hit)) {
			if(hit.collider.tag == "floor") {
				clickPoint = hit.point;
				clickedToMove = true;
				atDestination = false;
				print ("Hit the floor at" + hit.point);
			}
		}

		clickPoint.y = 0;
		
		if((clickPoint - my2DPosition).magnitude <= slideForgiveness) {
			atDestination = true;
			clickedToMove = false;
			rigidbody.velocity = Vector3.zero;
			print ("At destination");
		}

		if(clickedToMove == true && atDestination == false) {
			Vector3 direction = clickPoint - my2DPosition;
			direction.Normalize();
			if(rigidbody.velocity.magnitude < maxSpeed) {
				rigidbody.AddForce (direction * acceleration);
				print ("Adding Force to Move    " + direction * acceleration);
			}

		}



	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "enemy") {
			rigidbody.position = startingPosition;
			rigidbody.velocity = Vector3.zero;
			atDestination = true;
			clickedToMove = false;
		}
		if(collision.gameObject.name == "goal") {
			rigidbody.position = startingPosition;
			rigidbody.velocity = Vector3.zero;
			atDestination = true;
			clickedToMove = false;
		}
	}
}
