using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LetrasErradas : MonoBehaviour
{
    [SerializeField] GameObject ninho;
    private bool stopDrag = false;
    private bool drop = false;
    public Color objectColor;
    Vector3 mousePositionOffset;
    // Start is called before the first frame update
    void Start()
    {
        ResetP();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GetMouseWordPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseEnter()
    {
        
        GetComponent<SpriteRenderer>().color = new Color(objectColor.r, objectColor.g, objectColor.b, 0.5f);
    }

    private void OnMouseExit()
    {
        stopDrag = false;
        GetComponent<SpriteRenderer>().color = objectColor;
        drop = true;
    }

    private void OnMouseDrag()
    {
        if (!stopDrag)
        {
            transform.position = GetMouseWordPosition() + mousePositionOffset;
            
        }
    }

    private void OnMouseDown()
    {
        drop = false;
        mousePositionOffset = gameObject.transform.position - GetMouseWordPosition();
    }


    public void ResetP()
    {
        transform.position = ninho.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (drop && collision.gameObject.CompareTag("espaco"))
        {

            stopDrag = true;
            ResetP();



        }
        
        if (drop && collision.gameObject.CompareTag("espaco1"))
        {

            stopDrag = true;
            ResetP();



        }
        
        if (drop && collision.gameObject.CompareTag("espaco2"))
        {

            stopDrag = true;
            ResetP();



        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //stopDrag = false;
    }


}
