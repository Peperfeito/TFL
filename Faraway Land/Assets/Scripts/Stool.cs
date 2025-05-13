using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stool : MonoBehaviour
{
    public void Pusher(Vector3 pos)
    {
        transform.position += (transform.position - pos).normalized;
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 camilla = transform.position; //Camilla eh um buffer
        camilla.x = Mathf.Round(camilla.x) + .5f;
        camilla.y = Mathf.Round(camilla.y) + .3f;
        transform.position = camilla;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
