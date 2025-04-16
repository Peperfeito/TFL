using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetraI : MonoBehaviour
{
    [SerializeField] private GameObject letraI;
    public Color objectColor;
    Vector3 mousePositionOffset;
    private bool stopDrag = false;
    [SerializeField] GameObject ninho;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetP()
    {
        transform.position = ninho.transform.position;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("espaco2"))
        {

            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            letraI.SetActive(true);


        }
        else
        {
            stopDrag = true;
            ResetP();
        }


    }



}
