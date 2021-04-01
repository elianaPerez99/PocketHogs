using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogSpawner : MonoBehaviour {

	//variables
	public GameObject hhPrefab;
	public int maxHogsPer = 6;
	public int minHogsPer = 1;
	private int currentMax = 1;
	private int currentAmount = 0;
	private int currentNumClients = 0;
	public Vector2 spawnMin;
	public Vector2 spawnMax;


	//functions

	// Update is called once per frame
	private void Start()
	{
		//eventually we need to get the number of clients from the server, but for now we manually put it in
		ChangeNumHogs(1);
	}
	void Update () 
	{
		HedgeHog[] hogs = gameObject.GetComponentsInChildren<HedgeHog>();
		currentAmount = hogs.Length;
		AdjustHedgeHogs();
	}

	private void ChangeNumHogs(int numOfClients)
	{
		currentMax = (int)Random.Range(minHogsPer, maxHogsPer)*numOfClients;
	}

	//spawn hogs if there are too few
	private void SpawnHedgeHogs()
	{
		//later we need to spawn via types or potentially spawn types based on biome
		while (currentAmount < currentMax)
		{
			var position = new Vector3(Random.Range(spawnMin.x, spawnMax.x), Random.Range(spawnMin.y, spawnMax.y), 0);
			GameObject temp = Instantiate(hhPrefab, position, Quaternion.identity);
			temp.transform.SetParent(transform);
			currentAmount++;
		}
	}

	//delete them if there are too many
	private void DeleteHedgeHogs()
	{
		HedgeHog[] hogs = gameObject.GetComponentsInChildren<HedgeHog>();
		for (int i = hogs.Length-1; i >= currentMax; i--)
		{
			hogs[i].Destroy();
			currentAmount--;
		}
	}


	private void AdjustHedgeHogs()
	{
		if (currentMax < currentAmount)
		{
			DeleteHedgeHogs();
		}
		if (currentMax > currentAmount)
		{
			SpawnHedgeHogs();
		}
	}
}
