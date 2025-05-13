using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Invisiviu : MonoBehaviour
{
    public GameObject bars;
    SpriteRenderer barras;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        barras.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        barras.color = new Color(1, 1, 1, 1);   
    }

    // Start is called before the first frame update
    void Start()
    {
        barras = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
