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
public class ServerSpawner
{
    private List<hhData> hedgeHogs;
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
        hedgeHogs = new List<hhData>();
        maxHogsPer = 6;
        minHogsPer = 1;
        currentMax = 1;
        nextID = 0;
        currentAmount = 0;
        currentNumClients = 0;
        spawnMin = new Vector2(-7.4f, -4f);
        spawnMax = new Vector2(7.4f, 4f);
    }
    public void SpawnHedgeHogs(int clients)
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
            hedgeHogs.Add(temp);
            currentAmount++;
            nextID++;
        }
    }

    public List<hhData> GetList()
    {
        return hedgeHogs;
    }
}

