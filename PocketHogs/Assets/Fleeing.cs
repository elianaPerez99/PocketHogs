using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleeing : MonoBehaviour {
	private Vector3 targetLocation;
	public float speed = 5f;
	public bool areFleeing = false;
	private Rigidbody2D rb;
	public HedgeHog hog;
	// Use this for initialization
	void Start () 
	{
		rb = GetComponentInParent<Rigidbody2D>();
		hog = GetComponentInParent<HedgeHog>();
	}

    private void FixedUpdate()
    {
		if (areFleeing)
		{
			
		}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.CompareTag("Player"))
		{
			InvokeState();
		}
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			areFleeing = false;
			hog.ChangeState(HedgeHog.States.Wandering);
		}
	}

	public void InvokeState()
	{
		areFleeing = true;
		hog.ChangeState(HedgeHog.States.Fleeing);
	}
}
