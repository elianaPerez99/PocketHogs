using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hog
{
    // Hedgehog info goes here
}

public class HogPockets : MonoBehaviour
{
    // Array to hold hogs
    private Hog[] hogPockets = new Hog[6];

    private void Start()
    {
        
    }

    // Look for a free pocket
    public int FindFreePocket()
    {
        for (int i = 0; i < hogPockets.Length; i++)
        {
            if (hogPockets[i] == null)
            {
                // Return first free pocket num
                return i;
            }
        }

        // Return -1 if no free pocket found
        return -1;
    }

    // Adds a hog to your pockets
    public void AddHog(int pocket)
    {
        // Add hog information to pocket
        // hogPockets[pocket]
    }

    // Removes hog from inventory
    public void RemoveHog(int pocket)
    {
        hogPockets[pocket] = null;
    }
}
