using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> vehiclePrefabs = new List<GameObject>();

    public Direction direction;

    public int maxVehicles;
    public int vehiclesPerMinute;

    private int vehicleCount;
    private float timer;

    public void Start()
    {
        timer = 0;
        vehicleCount = 0;
        SpawnVehicle();
    }

    public void Update()
    {
        if(vehicleCount <= maxVehicles)
        {
            timer += Time.deltaTime;
            if(timer >= 60/vehiclesPerMinute)
            {
                timer = 0;

                SpawnVehicle();
                vehicleCount++;
            }
        }
    }

    public void SpawnVehicle()
    {
        var vehicleIndex = UnityEngine.Random.Range(0, vehiclePrefabs.Count);
        GameObject tempVehicle = Instantiate(vehiclePrefabs[vehicleIndex], transform.position, transform.rotation);
        tempVehicle.GetComponent<Vechicle>().initialMovementDirection = direction;
    }
}
