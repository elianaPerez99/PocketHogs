using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHedgeHogs : MonoBehaviour {
	public enum States
	{
		Wandering,
		SeekFood,
		EatFood,
		Fleeing
	};

    //variables
    private States currentState;

    //for multiple states
    private Vector3 targetLocation;
    public float baseSpeed = .5f;
    public hhData data;
    private float lastTime = 0f;

    //for wandering
    public Vector2 spawnMin;
    public Vector2 spawnMax;

    // Use this for initialization
    void Start () 
	{
            spawnMin = new Vector2(-7.4f, -4f);
             spawnMax = new Vector2(7.4f, 4f);
             ChangeState(States.Wandering);
            ChangeTarget();
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        switch (currentState)
        {
            case States.Wandering:
                Wander();
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
                Wander();
                break;
        }
	}

    public void ChangeState(States newState)
    {
        currentState = newState;
    }

    //Wandering Functions
    private void Wander()
    {
        Vector3 diff = targetLocation - transform.position;
        //velocity stuff
        if (diff.magnitude < 2)
        {
            ChangeTarget();
        }
        diff = new Vector2(diff.normalized.x, diff.normalized.y);
        diff *= baseSpeed;
        data.velocity = diff;
        GetComponent<Rigidbody2D>().velocity = diff;
        data.position = transform.position;
    }

    private void ChangeTarget()
    {
        targetLocation = new Vector3(Random.Range(spawnMin.x, spawnMax.x), Random.Range(spawnMin.y, spawnMax.y), 0);

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            ChangeTarget();
        }
    }

    //Fleeing Functions
    private void FleeTargetPosition(Vector3 playerPos)
    {
        
    }
    private void Fleeing()
    {
        Vector3 diff = targetLocation - transform.position;
        //velocity stuff
        if (diff.magnitude < 2)
        {
            ChangeState(States.Wandering);
        }
        diff = new Vector2(diff.normalized.x, diff.normalized.y);
        diff *= baseSpeed;
        data.velocity = diff;
        GetComponent<Rigidbody2D>().velocity = diff;
        data.position = transform.position;
    }
}
