using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private PocketUIScript pocketUI;

    private void Start()
    {
        pocketUI = GetComponentInChildren<Canvas>().GetComponentInChildren<PocketUIScript>();
    }

    private void Update()
    {
        for (int i = 0; i < hogPockets.Length; i++)
        {
            if ((hogPockets[i] as Hog) != null)
            {
                pocketUI.pocketSpaces[i].GetComponentsInChildren<Image>()[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                pocketUI.pocketSpaces[i].GetComponentInChildren<Text>().text = hogPockets[i].name;
            }
        }
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
        int randNum = Random.Range(0, names.Length);
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
