using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogSpawner : MonoBehaviour {

	//variables
	public GameObject hhPrefab;
	private float lastSentTime = 0f;
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
			string[] temp = tempStrArray[1].Substring(1, tempStrArray[1].Length - 2).Split(',');
			tempD.position = new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
			//getting rotation
			temp = tempStrArray[2].Substring(1, tempStrArray[2].Length - 2).Split(',');
			tempD.rotation = new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
			//getting velocity
			temp = tempStrArray[3].Substring(1, tempStrArray[3].Length - 2).Split(',');
			tempD.velocity = new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));

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
					hog.gameObject.transform.rotation = Quaternion.Euler(h.rotation.x, h.rotation.y, h.rotation.z);
					hog.gameObject.GetComponent<Rigidbody2D>().velocity = h.velocity;
				}
			}
			if (!exists)
			{
				GameObject newHoggie = Instantiate(hhPrefab, h.position, Quaternion.Euler(h.rotation.x, h.rotation.y, h.rotation.z),transform);
				newHoggie.GetComponent<HedgeHog>().id = h.id;
				newHoggie.GetComponent<HedgeHog>().newPosition = h.position;
			}

		}
		lastSentTime = sentTime;
	}

}
