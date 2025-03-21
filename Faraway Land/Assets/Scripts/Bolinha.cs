using System.Collections;
using System.Collections.Generic;
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
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startpoint].position;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Force > MaxForce)
        {
            Force = 0f;
        }

        if(press)
        {
            Force += Time.deltaTime * 10f;

            ForceBar.fillAmount = Force / MaxForce;
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

        }
    }

    private void OnMouseDown()
    {
        

        if (stop)
        {
            press = true;
        }

        stop = true;
    }

    private void OnMouseUp()
    {
        if (stop)
        {
            speed = 5;
            press = false;
            StartCoroutine(MoveForward());
        }
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
        
    }


}
