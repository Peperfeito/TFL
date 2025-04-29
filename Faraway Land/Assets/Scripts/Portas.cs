using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portas : MonoBehaviour
{

    [SerializeField] protected GameObject Desativar;
    [SerializeField] protected GameObject Ativar;

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
        if (collision.CompareTag("PlayerG"))
        {
            Ativar.SetActive(true);
            Desativar.SetActive(false);
            FindObjectOfType<FadeEffect>().FadeOut();
        }
    }
}
