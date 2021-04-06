using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class HedgeHogTest {

	[Test]
	public void CheckIfWanderingCorrectly()
	{
		//setting up fake hog
		GameObject hog = GameObject.FindGameObjectsWithTag("hog")[0];
		hog.GetComponent<HedgeHog>().wandering = hog.GetComponent<Wandering>();
		hog.GetComponent<HedgeHog>().ChangeState(HedgeHog.States.Wandering);
		//make sure the currentstate is being set correctly
		Assert.AreEqual(hog.GetComponent<HedgeHog>().currentState, HedgeHog.States.Wandering); // <-i made the enums before making the tests
		//make sure the wandering script is active
		Assert.IsTrue(hog.GetComponent<Wandering>().areWandering);

		//make sure that we dont have another state
		Assert.AreNotEqual(hog.GetComponent<HedgeHog>().currentState, HedgeHog.States.SeekFood);
		Assert.AreNotEqual(hog.GetComponent<HedgeHog>().currentState, HedgeHog.States.EatFood);
		Assert.AreNotEqual(hog.GetComponent<HedgeHog>().currentState, HedgeHog.States.Fleeing);

	}

	[Test]
	public void CheckIfFleeingCorrectly()
	{
		//setting up fake hog
		GameObject hog = GameObject.FindGameObjectsWithTag("hog")[0];
		HedgeHog hogScript = hog.GetComponent<HedgeHog>();
		Fleeing fleeScript = hog.GetComponentInChildren<Fleeing>();
		fleeScript.hog = hogScript;

		//add a player
		GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
		//put the player in the trigger's range
		player.transform.position = hog.transform.position;
		fleeScript.InvokeState();
		//make sure the state is now fleeing
		Assert.AreEqual(hogScript.currentState, HedgeHog.States.Fleeing);
		Assert.IsTrue(fleeScript.areFleeing);

		//make sure that we dont have another state
		Assert.AreNotEqual(hogScript.currentState, HedgeHog.States.SeekFood);
		Assert.AreNotEqual(hogScript.currentState, HedgeHog.States.EatFood);
		Assert.AreNotEqual(hogScript.currentState, HedgeHog.States.Wandering);

	}

	[Test]
	public void CheckIfSpawningCorrectly()
	{
		GameObject spawner = GameObject.FindGameObjectsWithTag("spawner")[0];
		int numSpawnedMax = spawner.GetComponent<HedgehogSpawner>().maxHogsPer;
		int numSpawnedMin = spawner.GetComponent<HedgehogSpawner>().minHogsPer;
		int test;
		//test at 1 client
		spawner.GetComponent<HedgehogSpawner>().ChangeNumHogs(1);
		test = spawner.GetComponent<HedgehogSpawner>().currentMax;
		Assert.IsTrue(test >= numSpawnedMin && test <= numSpawnedMax);
		//test at 2 clients
		spawner.GetComponent<HedgehogSpawner>().ChangeNumHogs(2);
		test = spawner.GetComponent<HedgehogSpawner>().currentMax;
		Assert.IsTrue(test >= numSpawnedMin*2 && test <= numSpawnedMax*2);

		//test at 3 clients
		spawner.GetComponent<HedgehogSpawner>().ChangeNumHogs(3);
		test = spawner.GetComponent<HedgehogSpawner>().currentMax;
		Assert.IsTrue(test >= numSpawnedMin*3 && test <= numSpawnedMax*3);

		//test at 4 clients
		spawner.GetComponent<HedgehogSpawner>().ChangeNumHogs(4);
		test = spawner.GetComponent<HedgehogSpawner>().currentMax;
		Assert.IsTrue(test >= numSpawnedMin*4 && test <= numSpawnedMax*4);

		//test at 5 clients
		spawner.GetComponent<HedgehogSpawner>().ChangeNumHogs(5);
		test = spawner.GetComponent<HedgehogSpawner>().currentMax;
		Assert.IsTrue(test >= numSpawnedMin*5 && test <= numSpawnedMax*5);

		//test at 6 clients
		spawner.GetComponent<HedgehogSpawner>().ChangeNumHogs(6);
		test = spawner.GetComponent<HedgehogSpawner>().currentMax;
		Assert.IsTrue(test >= numSpawnedMin*6 && test <= numSpawnedMax*6);
	}
}
