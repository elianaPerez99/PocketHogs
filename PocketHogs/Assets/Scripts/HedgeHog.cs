using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class HedgeHog : MonoBehaviour {
	//variables
	public enum States
	{
		Wandering,
		SeekFood,
		EatFood,
		Fleeing
	};

	public States currentState;
	public Wandering wandering;
	//functions
	// Use this for initialization
	void Start () 
	{
		currentState = States.Wandering;
		wandering = GetComponent<Wandering>();

		//right now this is in here but WE HAVE TO MOVE IT LATER (basically other components will call it)
		ChangeState(States.Wandering);
	}
	
	public void Destroy()
	{
		GameObject.Destroy(this);
	}

	public void ChangeState(States newState)
	{
		currentState = newState;
		switch (currentState)
		{
			case States.Wandering:
				wandering.ChangeIfWandering(true);
				break;
			case States.SeekFood:
				//put seekfood here
				break;
			case States.Fleeing:
				//put fleeing here
				break;
			case States.EatFood:
				//put eatfood here
				break;
			default:
				wandering.ChangeIfWandering(true);
				break;
		}
	}



}
