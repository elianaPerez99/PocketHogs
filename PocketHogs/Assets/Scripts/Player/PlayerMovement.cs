using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody rb;
	private float xMove;
	private float yMove;
	private float speed = 0.5f;



	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update ()
	{
		float xMove = Input.GetAxis("Horizontal") * speed;
		float yMove = Input.GetAxis("Vertical") * speed;

		rb.transform.Translate(xMove, yMove, 0);
	}
}
