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
    public float baseSpeed = .02f;
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
                Seeking();
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
        if (!collision.gameObject.CompareTag("Player")  && !collision.gameObject.CompareTag("Food") && currentState == States.Wandering)
        {
            ChangeTarget();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Food"))
        {
            ChangeState(States.SeekFood);
            SeekTargetPosition(col.gameObject.transform.position);
        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Food"))
        {
            ChangeState(States.Wandering);
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

    //seeking functions
    private void SeekTargetPosition(Vector3 foodPos)
    {
        targetLocation = foodPos;
    }

    private void Seeking()
    {
        Vector3 diff = targetLocation - transform.position;
        //velocity stuff
        if (!(diff.magnitude < 1))
        {
            diff = new Vector2(diff.normalized.x, diff.normalized.y);
            diff *= baseSpeed;     
        }
        else
        {
            diff = new Vector3(0f, 0f,0f);
        }
        data.velocity = diff;
        GetComponent<Rigidbody2D>().velocity = diff;
        data.position = transform.position;
    }
}
