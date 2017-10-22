using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{

    private float spawnTime = 1.5f;                                             // Define how long to wait until next balloon spawns
    public GameObject[] Balloons;                                               // Array Balloons
    public Transform[] spawnPoints;                                             // Array of the spawn points

    public List<Transform> possibleSpawns = new List<Transform>();              // Create a list of all the possible spawn locations available


    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            possibleSpawns.Add(spawnPoints[i]);
        }

        InvokeRepeating("SpawnBalloons", spawnTime, spawnTime);
    }

    // Define how/where the balloons shall spawn and retrieve the balloon sprites. Also get all possible areas for the next balloon to spawn in
    void SpawnBalloons()
    {
        if (possibleSpawns.Count > 0)
        {
            int spawnIndex = Random.Range(0, possibleSpawns.Count);
            int objectIndex = Random.Range(0, Balloons.Length);

            GameObject NewBalloon = Instantiate(Balloons[objectIndex], possibleSpawns[spawnIndex].position, possibleSpawns[spawnIndex].rotation) as GameObject;
            NewBalloon.GetComponent<Balloon>().newSpawnPoint = possibleSpawns[spawnIndex];

            possibleSpawns.RemoveAt(spawnIndex);
        }
    }
}
