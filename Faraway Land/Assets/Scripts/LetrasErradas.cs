using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LetrasErradas : MonoBehaviour
{
    [SerializeField] GameObject ninho;
    public bool stopDrag = false;
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
        stopDrag = false;
        GetComponent<SpriteRenderer>().color = new Color(objectColor.r, objectColor.g, objectColor.b, 0.5f);
    }

    private void OnMouseExit()
    {
       
        GetComponent<SpriteRenderer>().color = objectColor;
        
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
        
        mousePositionOffset = gameObject.transform.position - GetMouseWordPosition();
    }


    public void ResetP()
    {
        transform.position = ninho.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("espaco"))
        {

            stopDrag = true;
            ResetP();



        }
        
        if (collision.gameObject.CompareTag("espaco1"))
        {

            stopDrag = true;
            ResetP();



        }
        
        if (collision.gameObject.CompareTag("espaco2"))
        {

            stopDrag = true;
            ResetP();



        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
    }


}
