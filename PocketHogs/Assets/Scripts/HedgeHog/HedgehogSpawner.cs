using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogSpawner : MonoBehaviour {

	//variables
	public GameObject hhPrefab;
	private float lastSentTime = 0f;
	// Value for position value compression
	private float compressionVal = 100;
	//functions

	private List<hhData> GetNewDataFromServer(string msg)
	{
		List<hhData> hedgeHogDataList = new List<hhData>();

		string[] tempArray = msg.Split('`');
		foreach (string str in tempArray)
		{
			string[] tempStrArray = str.Split('~');
			hhData tempD;
			tempD.id = int.Parse(tempStrArray[0]);
			//getting position
			tempD.position = new Vector3(DecompressPosFloat(int.Parse(tempStrArray[1])), DecompressPosFloat(int.Parse(tempStrArray[2])), 0f);
			//getting velocity
			tempD.velocity = new Vector3(DecompressPosFloat(int.Parse(tempStrArray[3])), DecompressPosFloat(int.Parse(tempStrArray[4])), 0f);

			hedgeHogDataList.Add(tempD);
		}

		return hedgeHogDataList;
	}

	public void UpdateHogs(string msg, float sentTime)
	{
		List<hhData> list = GetNewDataFromServer(msg);
		HedgeHog[] hogs = gameObject.GetComponentsInChildren<HedgeHog>();
		
		foreach (hhData h in list)
		{
			bool exists = false;
			foreach (HedgeHog hog in hogs)
			{
				if (h.id == hog.id)
				{
					exists = true;
					//move this to an update function that deals with smoothness
					//hog.gameObject.transform.position = h.position;
					hog.newPosition = h.position;
					hog.gameObject.GetComponent<Rigidbody2D>().velocity = h.velocity;
				}
			}
			if (!exists)
			{
				GameObject newHoggie = Instantiate(hhPrefab, transform);
				newHoggie.transform.position = new Vector3(h.position.x, h.position.y, 0);
				newHoggie.GetComponent<HedgeHog>().id = h.id;
				newHoggie.GetComponent<HedgeHog>().newPosition = h.position;
			}
		}

		if (hogs.Length > list.Count)
		{
			for (int i = 0; i < hogs.Length; i++)
			{
				bool exists = false;
				foreach (hhData h in list)
				{
					if (h.id == hogs[i].id)
					{
						exists = true;
					}
				}
				if (!exists)
				{
					hogs[i].Destroy();
				}
			}
		}
		

		lastSentTime = sentTime;
	}
	// Decompress position data
	private float DecompressPosFloat(int x)
	{
		return (float)x / compressionVal;
	}
}
