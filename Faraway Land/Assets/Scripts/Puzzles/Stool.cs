using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stool : MonoBehaviour
{
    public Vector3 initialPos;
    /*public Vector3 restartPos;*/
    public float xMin = -14.5f;
    public float xMax = 14.5f;
    public float yMin = -17.5f;
    public float yMax = 1.5f;
    

    public void Restarter()
    {
        if ((this.transform.position.x < xMin) || (this.transform.position.x >= xMax) || (this.transform.position.y < yMin) || (this.transform.position.y >= yMax))
        {
            this.transform.position = initialPos;
        }
        else
        {
            return;
        }
    }
    /*public void RestartPosition()
    {
        if (this.transform.position == restartPos)
        {
            this.transform.position = initialPos;
        }

        else
        {
            return;
        }
    }*/
    public void Pusher(Vector3 pos)
    {
        transform.position += (transform.position - pos).normalized;
        Vector3 camilla = transform.position; //Camilla eh um buffer
        camilla.x = Mathf.Round(camilla.x) + .49f;
        camilla.y = Mathf.Round(camilla.y) + .3f;
        transform.position = camilla;
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 camilla = transform.position; //Camilla eh um buffer
        camilla.x = Mathf.Round(camilla.x) + .5f;
        camilla.y = Mathf.Round(camilla.y) + .3f;
        transform.position = camilla;
        initialPos = camilla;
    }

    // Update is called once per frame
    void Update()
    {
        Restarter();
    }
}
