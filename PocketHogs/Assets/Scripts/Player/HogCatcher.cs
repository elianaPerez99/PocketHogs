using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HogCatcher : MonoBehaviour
{
    int recentID;
    private void Start()
    {
        recentID = -1;
    }
    private void OnTriggerEnter2D(Collider2D hog)
    {
        // Begin capture process if the collision is with a hog
        if(hog.CompareTag("Hog"))
        {
            // Check if there's a free pocket
            int pocket = GetComponentInParent<HogPockets>().FindFreePocket();

            // If there's a free pocket, we catch the hog
            if (pocket >= 0 && hog.GetComponent<HedgeHog>().id != recentID)
            {
                recentID = hog.GetComponent<HedgeHog>().id;
                GetComponentInParent<HogPockets>().AddHog(pocket);
                //inform client that hedgheog is now gone
                GameObject.Find("Client").GetComponent<Client>().SendDestructionOfBoi(hog.GetComponent<HedgeHog>().id);
                hog.GetComponent<HedgeHog>().Destroy();
            }
        }
    }
}
