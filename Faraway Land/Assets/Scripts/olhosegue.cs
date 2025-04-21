using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class olhosegue : MonoBehaviour
{
    public GameObject Player;
    public GameObject binho;
    public float spid;
    float distance;
    public float minDistance = 2;
    public float maxDistance = 4;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //paunocudequem
    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, Player.transform.position);
        if (distance < minDistance) {
            transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, spid*Time.deltaTime);
        }
        if (distance > maxDistance)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, binho.transform.position, spid * Time.deltaTime);
            //transform.position = binho.transform.position;
        } 
    }
}
