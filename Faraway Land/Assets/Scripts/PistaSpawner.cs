using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistaSpawner : MonoBehaviour
{
    public GameObject roadPrefab;
    public int numberOfRoads = 5;
    public float roadLength = 5f;
    public float speed = 2f;
    public GameObject roadContent;

    private List<GameObject> roadPieces = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfRoads; i++)
        {
            //Vector3 spawnPos = new Vector3(0, i * roadLength, 0); 
            GameObject road = Instantiate(roadPrefab, roadContent.transform);
            roadPieces.Add(road);
            Debug.Log("funciona");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < roadPieces.Count; i++)
        {
            roadPieces[i].transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        if (roadPieces[0].transform.position.y < -roadLength)
        {
            GameObject oldRoad = roadPieces[0]; 
            roadPieces.RemoveAt(0); 

            
            GameObject lastRoad = roadPieces[roadPieces.Count - 1];
            Vector3 newPosition = lastRoad.transform.position + new Vector3(0, roadLength, 0);
            oldRoad.transform.position = newPosition;

            roadPieces.Add(oldRoad); 
        }
    }
}
