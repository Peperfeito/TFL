using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistaSpawner : MonoBehaviour
{
    public GameObject roadPrefab;
    public int numberOfRoads = 5;
    public float roadLength = 10f;
    public float speed = 10f;

    private Queue<GameObject> roadPieces = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfRoads; i++)
        {
            Vector3 spawnPos = new Vector3(0, 0, i * roadLength);
            GameObject road = Instantiate(roadPrefab, spawnPos, Quaternion.identity);
            roadPieces.Enqueue(road);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject road in roadPieces)
        {
            road.transform.position += Vector3.back * speed * Time.deltaTime;
        }

        // Verificar se a pista mais antiga saiu da tela e precisa ser reciclada
        if (roadPieces.Peek().transform.position.z < -roadLength)
        {
            GameObject oldRoad = roadPieces.Dequeue();
            oldRoad.transform.position = roadPieces.ToArray()[roadPieces.Count - 1].transform.position + new Vector3(0, 0, roadLength);
            roadPieces.Enqueue(oldRoad);
        }
    }
}
