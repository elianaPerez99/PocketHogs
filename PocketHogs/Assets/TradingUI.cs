using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TradingUI : MonoBehaviour {


	public void SetPockets(HogPockets hp)
	{
		Button[] buttons = GetComponentsInChildren<Button>();
		for (int i = 0; i < buttons.Length; i++)
		{
			if ((hp.GetHogs()[i] as Hog) != null)
			{
				buttons[i].GetComponentInChildren<Text>().text = hp.GetHogs()[i].name;
				buttons[i].GetComponent<TradeHog>().index = i;
				buttons[i].interactable = true;
			}
			else
			{
				buttons[i].GetComponentInChildren<Text>().text = "";
				buttons[i].GetComponent<TradeHog>().index = i;
				buttons[i].interactable = false;
			}
		}

	}
}
