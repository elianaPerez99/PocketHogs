using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody rb;
	private float xMove;
	private float yMove;
	private float speed = 5.0f;

	Camera gameCamera;


	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		gameCamera = FindObjectOfType<Camera>();
	}

	// Update is called once per frame
	void Update ()
	{
		float xMove = Input.GetAxis("Horizontal") * speed;
		float yMove = Input.GetAxis("Vertical") * speed;

		rb.velocity = new Vector3(xMove, yMove, 0);

		CameraFollow();
	}

	// Make camera follow moveable player
	private void CameraFollow()
	{
		Vector3 position = gameCamera.transform.position;
		position.y = transform.position.y;
		position.x = transform.position.x;

		gameCamera.transform.position = position;
	}
}
