using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afk : MonoBehaviour
{
    private new CameraSmooth camera;
    [SerializeField] private Transform novoalvo;

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
            Debug.Log(novoalvo.gameObject);
            camera.Setarget(novoalvo);
            
        }

    }
}
