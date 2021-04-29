using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgeHog : MonoBehaviour
{
    //variables
    public int id;
    public Vector3 newPosition;
    void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, newPosition.x, .1f), Mathf.Lerp(transform.position.y, newPosition.y, .1f),0);
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }


}
