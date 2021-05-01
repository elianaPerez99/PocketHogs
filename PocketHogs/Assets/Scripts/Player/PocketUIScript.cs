using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PocketUIScript : MonoBehaviour {
	public VerticalLayoutGroup[] pocketSpaces;
	// Use this for initialization
	void Start () 
	{
		pocketSpaces = GetComponentsInChildren<VerticalLayoutGroup>();
	}
	
}
