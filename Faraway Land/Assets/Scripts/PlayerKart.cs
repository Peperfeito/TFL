using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerKart : MonoBehaviour
{
    public float speed = 10f;
    public float laneWidth = 0.5f;
    private int lane = 0;
    private Vector3 startPosition;



    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && lane > -1)
        {
            lane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && lane < 1)
        {
            lane++;
        }

        
        transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * speed);
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }

    }




}
