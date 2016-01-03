﻿using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {

	public Transform[] patrolPoints = new Transform[2];
	public float moveSpeed;
	private int currentPoint;

	// Use this for initialization
	void Start () {
		transform.position = patrolPoints[0].position;
		currentPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position == patrolPoints[currentPoint].position) {
			currentPoint = (currentPoint + 1) % patrolPoints.Length;
		}
		transform.position = Vector3.MoveTowards (transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime);

	
	}
}
