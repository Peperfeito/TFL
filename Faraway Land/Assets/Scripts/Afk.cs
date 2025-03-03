using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afk : MonoBehaviour
{
    private CameraSmooth cameraSmooth;
    [SerializeField] private Transform novoalvo;
    public bool ativar = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        


        if (collision.CompareTag("Player"))
        {
            ativar = true;
           
            
        }

    }
}
