using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portas : MonoBehaviour
{

    [SerializeField] protected GameObject destino;
    [SerializeField] private GameObject cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.Find("MovePoint").transform.position = destino.transform.position;
            collision.transform.position = destino.transform.position;
            cam.transform.position = destino.transform.position;
            FindObjectOfType<FadeEffect>().FadeOut();
        }
    }
}
