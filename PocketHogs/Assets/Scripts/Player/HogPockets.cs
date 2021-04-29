using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hog
{
    // Hedgehog info goes here
    public string name;
}

public class HogPockets : MonoBehaviour
{
    // Array to hold hogs
    private Hog[] hogPockets = new Hog[6];

    private string[] names = new string[]{"Froderick", "Jerry", "Alyssa", "Patty", "Cupcake", "Sonic", "Shadow", "Knuckles"};

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
        // Add random hog information to pocket
        Hog hoggyBoi = new Hog();
        int randNum = Random.Range(0, names.Length - 1);
        hoggyBoi.name = names[randNum];
        hogPockets[pocket] = hoggyBoi;

        Debug.Log("Obtained the boi " + hogPockets[pocket].name);
    }

    // Removes hog from inventory
    public void RemoveHog(int pocket)
    {
        hogPockets[pocket] = null;
    }
}
