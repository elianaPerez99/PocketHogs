using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : MonoBehaviour {
	private Vector3 targetLocation;
	public Vector2 spawnMin;
	public Vector2 spawnMax;
	public float speed = 5f;
	public bool areWandering = true;
	private Rigidbody2D rb;

	//tutorial stuff (delete if no worky)
	private bool isMoving;
	private Vector3 currentPos, gridTarget;
	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
		ChangeTarget();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (areWandering)
		{
			Vector3 diff = targetLocation - transform.position;


			//velocity stuff
			if (diff.magnitude < 1)
			{
				ChangeTarget();
			}
			diff = new Vector2(diff.normalized.x, diff.normalized.y);
			diff *= speed;
			rb.velocity = diff;
			


		}
		
	}

	public void ChangeIfWandering(bool x)
    {
		areWandering = x;
    }
	private void ChangeTarget()
	{
		targetLocation = new Vector3(Random.Range(spawnMin.x, spawnMax.x), Random.Range(spawnMin.y, spawnMax.y), 0);
	}

	//if they collide with something they change direction
	void OnCollisionStay2D(Collision2D collision)
	{
		ChangeTarget();
	}
}
