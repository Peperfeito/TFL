using System.Collections.Generic;
using UnityEngine;

public class PistaSpawner : MonoBehaviour
{
    public GameObject roadPrefab;
    public int numberOfRoads = 5;
    public float roadLength = 5f;
    public float speed = 2f;
    public GameObject spawnPoint;
    

    private List<GameObject> roadPieces = new List<GameObject>();

    void Start()
    {
        Vector3 spawnTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, Camera.main.nearClipPlane + 1f));
        spawnTop.z = 0f;

        for (int i = 0; i < numberOfRoads; i++)
        {
            Vector3 spawnPosition = spawnTop - new Vector3(0, i * roadLength, 0); // <- espaçamento
            GameObject road = Instantiate(roadPrefab, spawnPosition, Quaternion.identity);
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
        /*if (roadPieces[0].transform.position.y < -30f)
        {
            GameObject oldRoad = roadPieces[0];
            roadPieces.RemoveAt(0);

            GameObject lastRoad = roadPieces[roadPieces.Count - 1];

            Pista lastPista = lastRoad.GetComponent<Pista>();
            Pista oldPista = oldRoad.GetComponent<Pista>();

            float lastHeight = lastPista != null ? lastPista.GetScaledHeight() : roadLength;
            float oldHeight = oldPista != null ? oldPista.GetScaledHeight() : roadLength;

            Vector3 newPos = lastRoad.transform.position + new Vector3(0, (lastHeight + oldHeight) / 2f, 0);

            oldRoad.transform.position = newPos;

            roadPieces.Add(oldRoad);
        }*/
    }
}
