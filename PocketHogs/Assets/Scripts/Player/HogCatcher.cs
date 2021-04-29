using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HogCatcher : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D hog)
    {
        // Begin capture process if the collision is with a hog
        if(hog.CompareTag("Hog"))
        {
            // Check if there's a free pocket
            int pocket = GetComponentInParent<HogPockets>().FindFreePocket();

            // If there's a free pocket, we catch the hog
            if (pocket >= 0)
            {
                GetComponentInParent<HogPockets>().AddHog(pocket);
                hog.GetComponent<HedgeHog>().Destroy();
            }
        }
    }
}
