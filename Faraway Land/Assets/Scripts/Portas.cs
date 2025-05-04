using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portas : MonoBehaviour
{

    [SerializeField] protected GameObject destino;
    [SerializeField] private GameObject cam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = destino.transform.position;
            cam.transform.position = destino.transform.position;
            FindObjectOfType<FadeEffect>().FadeOut();
        }
    }
}
