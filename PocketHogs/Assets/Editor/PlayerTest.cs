using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class PlayerTest{

	[Test]
	public void TestPlayerMovement()
	{
		GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
		float originalY = player.transform.position.y;
		float originalX = player.transform.position.x;

		//make it move right
		player.GetComponent<PlayerMovement>().SetMovementSpeed(1.0f, 0.0f);
		Assert.That(player.transform.position.x > originalX);

		//make it move left
		originalX = player.transform.position.x;
		player.GetComponent<PlayerMovement>().SetMovementSpeed(-1.0f, 0.0f);
		Assert.That(player.transform.position.x < originalX);


		//make it move up
		player.GetComponent<PlayerMovement>().SetMovementSpeed(0.0f, 1.0f);
		Assert.That(player.transform.position.y > originalY);

		//make it move down
		originalY = player.transform.position.y;
		player.GetComponent<PlayerMovement>().SetMovementSpeed(0.0f, -1.0f);
		Assert.That(player.transform.position.y < originalY);
	}
}
