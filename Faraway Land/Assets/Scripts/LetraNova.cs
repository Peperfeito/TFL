using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static LetraNova;

public class LetraNova : MonoBehaviour
{
    
    public bool stopDrag = false;
    public Color objectColor;
    Vector3 mousePositionOffset;
    
    public enum Letra {M, E, I , Errada }
    public Letra minhaLetra;
    public GameObject objetoCerto;
    SpriteRenderer lateral;
    Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        lateral = GetComponent<SpriteRenderer>();
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

    }

    private void OnMouseUp()
    {
        lateral.color = Color.white;
        rigidbody2D.gravityScale = 1;


    }

    private void OnMouseDrag()
    {
        if (!stopDrag)
        {
            transform.position = GetMouseWordPosition() + mousePositionOffset;
            rigidbody2D.gravityScale = 0;
            
        }
    }

    private void OnMouseDown()
    {
        lateral.color = objectColor;
        mousePositionOffset = gameObject.transform.position - GetMouseWordPosition();
        
    }


    public void ResetP()
    {
        transform.position = transform.parent.position;
        lateral.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (minhaLetra)
        {
            case Letra.M:
                if (collision.gameObject.CompareTag("espaco"))
                {

                    collision.gameObject.SetActive(false);
                    
                    objetoCerto.SetActive(true);
                    VerificarLetra();
                    
                }
                if (collision.gameObject.CompareTag("Vazio"))
                {

                    stopDrag = true;
                    ResetP();



                }
                stopDrag = true;
                ResetP();
                break;

            case Letra.E:
                if (collision.gameObject.CompareTag("espaco1"))
                {

                    collision.gameObject.SetActive(false);
                    
                    objetoCerto.SetActive(true);
                    VerificarLetra();


                }
                if (collision.gameObject.CompareTag("Vazio"))
                {

                    stopDrag = true;
                    ResetP();



                }
                stopDrag = true;
                    ResetP();
       
                break;
            
            case Letra.I:
                if (collision.gameObject.CompareTag("espaco2"))
                {

                    collision.gameObject.SetActive(false);
                    
                    objetoCerto.SetActive(true);
                    VerificarLetra();


                }
                if (collision.gameObject.CompareTag("Vazio"))
                {

                    stopDrag = true;
                    ResetP();



                }
                stopDrag = true;
                    ResetP();
                break;

            case Letra.Errada:
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
                if (collision.gameObject.CompareTag("Vazio"))
                {

                    stopDrag = true;
                    ResetP();



                }

                break;

        }

        

        

        

    }

    public void VerificarLetra()
    {
        Afk afkalsons = GameObject.FindAnyObjectByType<Afk>();
        afkalsons.Verificar();
    }
    private void OnTriggerExit2D(Collider2D other)
    {

    }
}
