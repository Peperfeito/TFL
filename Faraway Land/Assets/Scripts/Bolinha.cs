using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bolinha : MonoBehaviour
{

    [SerializeField] Image ForceBar;
    [SerializeField] float Force;
    [SerializeField] float MaxForce = 100;
    public int startpoint;
    public Transform[] points;
    public float speed;
    private bool press;
    private Vector3 lastPosition;
    private int i;
    public Vector3 direction;
    private bool stop = false;
    private bool errou = false;
    private bool move = true;
    [SerializeField] GameObject spawn;
    


    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startpoint].position;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posAtual = transform.position;

        bool xParado = Mathf.Approximately(posAtual.x, lastPosition.x);
        bool yParado = Mathf.Approximately(posAtual.y, lastPosition.y);

        if (xParado && yParado)
        {
            ReseteBola();
        }

        if (move)
        {
            if (Input.GetKey("space"))
            {
                Force += Time.deltaTime * 10f;
                ForceBar.fillAmount = Force / MaxForce;
                if (Force > MaxForce)
                {
                    Force = 0f;
                }

                stop = true;
                press = true;
            }
        }
        if (press)
        {
            if (Input.GetKeyUp("space"))
            {
                move = false;
                speed = 5;
                StartCoroutine(MoveForward());
                Force = 0;
                ForceBar.fillAmount = 0;

            }
        }

            
        
        if (!stop)
        {
            Vector3 delta = transform.position - lastPosition;

            direction = delta.normalized;


            lastPosition = transform.position;

            if (Vector2.Distance(transform.position, points[i].position) < 0.05f)
            {
                i++;
                if (i == points.Length)
                {
                    i = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
            move = true;
        }
    }

    private void OnMouseDown()
    {
        

       
    }

    public void ReseteBola()
    {
        transform.position = points[startpoint].position;
        move = false;
        stop = false;
        errou = false;
        speed = 20;
        press = false;


    }

    private void OnMouseUp()
    {
        
    }

    IEnumerator MoveForward()
    {


        

        Vector3 targetPosition = transform.position + new Vector3(0, Force, 0);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            

            yield return null;
        }



        transform.position = targetPosition;

        yield return new WaitForSeconds(1f);

        //transform.position = lastPosition;
        if (transform.position == targetPosition)
        {
            ReseteBola();
            
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fora"))
        {
            StopAllCoroutines();
            transform.position = spawn.transform.position;
            ReseteBola();
            
        }

        if (collision.gameObject.CompareTag("Objetivo"))
        {
            //Destroy(gameObject);// diminuir a hit box de ambos
            ReseteBola();
        }
    }

    private void StopAllCoroutines(IEnumerator enumerator)
    {
        throw new NotImplementedException();
    }
}
