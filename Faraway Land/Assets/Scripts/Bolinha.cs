using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bolinha : MonoBehaviour
{

    [SerializeField] Image forceBar;
    [SerializeField] float force;
    [SerializeField] float maxforce = 100;
    public int startpoint;
    public Transform[] points;
    public float speed;
    private bool press;
    private Vector3 lastPosition;
    private int i;
    public Vector3 direction;
    private bool stop = false;
    private bool move = true;
    [SerializeField] GameObject spawn;
    private Camera cameraPlayer;
    [SerializeField] private GameObject paiacuMinigame;
    [SerializeField] private GameObject jail;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject barrinha;
    [SerializeField] private GameObject portaKano;
    public Transform alvo; 
    public float distanciaMaxima = 20f; 
    public float escalaMinima = 0.02f;
    private float multi = 100f;
    Animator clownPuzzleAnimator;

    private Vector3 escalaOriginal;



    // Start is called before the first frame update
    void Start()
    {
        
        clownPuzzleAnimator = GameObject.Find("Geovanna").GetComponent<Animator>();
        transform.position = points[startpoint].position;
        lastPosition = transform.position;
        cameraPlayer = Camera.main;
        escalaOriginal = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (alvo == null) return;

        float distancia = Vector3.Distance(transform.position, alvo.position);
        float t = Mathf.Clamp01(distancia / distanciaMaxima); 

        float escalaAtual = Mathf.Lerp(escalaMinima, 1f, t);
        transform.localScale = escalaOriginal * escalaAtual;




        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paiacuMinigame.SetActive(false);
            cameraPlayer.gameObject.SetActive(true);
            barrinha.SetActive(false);
        }

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
                force += Time.deltaTime * multi;
                forceBar.fillAmount = force / maxforce;
                if (force >= maxforce || force <= 0)
                {
                    multi *= -1;
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
                force = 0;
                forceBar.fillAmount = 0;
                
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

    public void ReseteBola()
    {
        transform.position = points[startpoint].position;
        move = false;
        stop = false;
        speed = 20;
        press = false;


    }

    IEnumerator MoveForward()
    {


        

        Vector3 targetPosition = transform.position + new Vector3(0, force, 0);

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
            
            
            jail.SetActive(false);
            clownPuzzleAnimator.Play("ClownPuzzleFinishedAnim");
            Destroy(gameObject);
            cam.SetActive(true);
            barrinha.SetActive(false);
            portaKano.SetActive(true);
            paiacuMinigame.SetActive(false);

        }
    }

    private void StopAllCoroutines(IEnumerator enumerator)
    {
        throw new NotImplementedException();
    }
}
