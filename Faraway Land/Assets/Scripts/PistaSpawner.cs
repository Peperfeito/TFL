using System.Collections.Generic;
using UnityEngine;

public class PistaSpawner : MonoBehaviour
{
    public GameObject roadPrefab;
    public int numberOfRoads = 5;
    public float roadLength = 5f;
    public float speed = 2f;

    private List<GameObject> roadPieces = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < numberOfRoads; i++)
        {
            Vector3 spawnPos = new Vector3(0, i * roadLength, 0);
            GameObject road = Instantiate(roadPrefab, spawnPos, Quaternion.identity);
            roadPieces.Add(road);
        }
    }

    void Update()
    {
        foreach (GameObject road in roadPieces)
        {
            road.transform.position += Vector3.down * speed * Time.deltaTime;
        }

        // Recicla a primeira pista
        if (roadPieces[0].transform.position.y < -roadLength)
        {
            GameObject oldRoad = roadPieces[0];
            roadPieces.RemoveAt(0);

            GameObject lastRoad = roadPieces[roadPieces.Count - 1];
            Vector3 newPos = lastRoad.transform.position + new Vector3(0, roadLength, 0);
            oldRoad.transform.position = newPos;

            roadPieces.Add(oldRoad);
        }
    }
}
