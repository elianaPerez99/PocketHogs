using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct hhData
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 velocity;
    public int id;
}
public class ServerSpawner:MonoBehaviour
{
    private List<GameObject> hedgeHogCalculations;
    public GameObject hhprefab;
    public int maxHogsPer;
    public int minHogsPer;
    private int currentMax;
    private int nextID;
    private int currentAmount;
    private int currentNumClients;
    public Vector2 spawnMin;
    public Vector2 spawnMax;

    public ServerSpawner()
    {
        maxHogsPer = 4;
        minHogsPer = 2;
        currentMax = 4;
        nextID = 0;
        currentAmount = 0;
        currentNumClients = 0;
        spawnMin = new Vector2(-7.4f, -4f);
        spawnMax = new Vector2(7.4f, 4f);
        hedgeHogCalculations = new List<GameObject>();
    }

    public void SpawnHedgeHogs(int clients)
    {
        if (currentNumClients < clients)
        {
            currentNumClients = clients;
            currentMax = (int)Random.Range(minHogsPer, maxHogsPer) * currentNumClients;

            while (currentAmount < currentMax)
            {
                var position = new Vector3(Random.Range(spawnMin.x, spawnMax.x), Random.Range(spawnMin.y, spawnMax.y), 0);
                hhData temp = new hhData();
                temp.position = position;
                temp.rotation = new Vector3(0, 0, 0);
                temp.velocity = new Vector3(0, 0, 0);
                temp.id = nextID;
                GameObject tempObj = Instantiate(hhprefab, temp.position, Quaternion.Euler(temp.rotation), transform);
                tempObj.GetComponent<ServerHedgeHogs>().data = temp;
                hedgeHogCalculations.Add(tempObj);

                currentAmount++;
                nextID++;
            }
        }
        else if (currentAmount < minHogsPer*clients || currentAmount == 0)
        {
            currentNumClients = clients;
            while (currentAmount < currentMax)
            {
                var position = new Vector3(Random.Range(spawnMin.x, spawnMax.x), Random.Range(spawnMin.y, spawnMax.y), 0);
                hhData temp = new hhData();
                temp.position = position;
                temp.rotation = new Vector3(0, 0, 0);
                temp.velocity = new Vector3(0, 0, 0);
                temp.id = nextID;
                GameObject tempObj = Instantiate(hhprefab, temp.position, Quaternion.Euler(temp.rotation), transform);
                tempObj.GetComponent<ServerHedgeHogs>().data = temp;
                hedgeHogCalculations.Add(tempObj);

                currentAmount++;
                nextID++;
            }
        }
    }

    public void DeleteBoi(int id)
    {
        foreach (GameObject go in hedgeHogCalculations)
        {
            if (go.GetComponent<ServerHedgeHogs>().data.id == id)
            {
                currentAmount--;
                hedgeHogCalculations.Remove(go);
                Destroy(go);

                break;
            }
        }

        SpawnHedgeHogs(currentNumClients);
    }

    public List<hhData> GetList()
    {
        List<hhData> tempList = new List<hhData>();
        foreach (GameObject shh in hedgeHogCalculations)
        {
            tempList.Add(shh.GetComponent<ServerHedgeHogs>().data);
        }
        return tempList;
    }
}

