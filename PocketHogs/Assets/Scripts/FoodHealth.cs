using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHealth : MonoBehaviour {
	private int id;
	private int health = 10;
	private bool nearHog = false;
	private bool eating = false;
	private int hogsNear = 0;

	void Update()
	{
		if (nearHog && !eating)
		{
			StartCoroutine(DecreaseHealth());
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("SHog"))
		{
			hogsNear++;
			nearHog = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("SHog"))
		{
			hogsNear--;
			if (hogsNear <= 0)
			{
				nearHog = false;
				eating = false;
			}
		}
	}

	private IEnumerator DecreaseHealth()
	{
		eating = true;
		while (health != 0)
		{
			health -= 1;
			yield return new WaitForSeconds(2f);
		}
		if (health == 0)
		{
			Destroy();
		}
	}

	private void Destroy()
    {
		//SEND HAS DESTROYED SOMEHOW TO THE SERVER SO THE PLAYERS KNOW
		GameObject.Destroy(gameObject);
    }

	public void SetId(int newId)
	{
		id = newId;
	}

	public int GetId()
	{
		return id;
	}
}
