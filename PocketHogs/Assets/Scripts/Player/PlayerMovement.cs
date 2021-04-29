﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	// For player movement
	private Rigidbody rb;
	private float xMove;
	private float yMove;
	private float speed = 5.0f;

	// Game camera
	private Camera gameCamera;

	// Check if moved
	private Vector3 lastPos;

	// Food information
	public GameObject foodPrefab;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		gameCamera = FindObjectOfType<Camera>();

		lastPos = transform.position;
	}

	// Update is called once per frame
	void Update ()
	{
		GetInput();
		CameraFollow();
	}

	// Get player inputs
	private void GetInput()
	{
		// Input for walking
		float xMove = Input.GetAxis("Horizontal") * speed;
		float yMove = Input.GetAxis("Vertical") * speed;

		rb.velocity = new Vector3(xMove, yMove, 0);

		// Add input for dropping food
		if(Input.GetKeyDown(KeyCode.F))
		{
			DropFood();
		}
	}

	// Player drops food nearby for hedgehogs
	private void DropFood()
	{
		GameObject food = Instantiate(foodPrefab) as GameObject;
		food.transform.position = transform.position;
	}

	// Make camera follow moveable player
	private void CameraFollow()
	{
		Vector3 position = gameCamera.transform.position;
		position.y = transform.position.y;
		position.x = transform.position.x;

		gameCamera.transform.position = position;
	}

	// Check if moved since last update
	public bool HasMoved()
	{
		return lastPos != transform.position;
	}

	// Sets new position update check
	public void SetLastPos()
	{
		lastPos = transform.position;
	}
}
