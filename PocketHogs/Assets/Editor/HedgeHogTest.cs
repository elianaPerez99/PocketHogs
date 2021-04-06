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
