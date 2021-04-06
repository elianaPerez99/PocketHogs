using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	Rigidbody2D rb;
	public float moveSpeed = 2f;
	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
		SetMovementSpeed(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public void SetMovementSpeed(float horizontal, float vertical)
	{
		Vector3 movement = new Vector3(horizontal, vertical, 0);
		transform.position += movement * Time.deltaTime * moveSpeed;
	}
}
