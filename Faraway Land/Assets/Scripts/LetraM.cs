using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetraM : MonoBehaviour
{

    [SerializeField] private GameObject letraM;
    public Color objectColor;
    Vector3 mousePositionOffset;
    private bool stopDrag = false;
    [SerializeField] GameObject ninho;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = objectColor;
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
        

        if (collision.gameObject.CompareTag("espaco"))
        {
            
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            letraM.SetActive(true);


        }
        else
        {
            stopDrag = true;
            ResetP();
        }


    }

    


}
