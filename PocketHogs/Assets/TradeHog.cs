using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeHog : MonoBehaviour {
	public int index = 0;
	public void ChooseHog()
	{
		Client client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
		HogPockets pockets = client.players[client.myClientId].avatar.GetComponent<HogPockets>();
		client.TradeHog(GetComponentInChildren<Text>().text);
		Hog[] hogs = pockets.GetHogs();
		Debug.Log("chosen hog");

		hogs[index] = null;
		pockets.SetHogs(hogs);
		GetComponentInParent<TradingUI>().SetPockets(pockets);
		

	}
}
