using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	float xMove;
	float yMove;

	// Update is called once per frame
	void Update ()
	{
		CheckInput();
		UpdatePos();
	}

	private void CheckInput()
	{
		float xMove = Input.GetAxis("Horizontal");
		float yMove = Input.GetAxis("Vertical");
	}

	private void UpdatePos()
	{
		transform.Translate(xMove, yMove, 0);
	}
}
