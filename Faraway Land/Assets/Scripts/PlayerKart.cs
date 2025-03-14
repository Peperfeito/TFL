using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerKart : MonoBehaviour
{
    public LayerMask colisores;
    public Transform movePoint;
    private float horizontal;
    private float vertical;
    public float moveSpeed = 7f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        AndaGrid();
    }

    private void AndaGrid()
    {
        bool moveflag = (transform.position - movePoint.position).magnitude > 0.2f;
        Vector2 directionRayCast = new Vector2(horizontal, vertical);

        if (!moveflag && Physics2D.Raycast(transform.position, directionRayCast, 1f, colisores))
        {
            Debug.DrawRay(transform.position, directionRayCast);
            return;


        }




        

        

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {

            if (Mathf.Abs(horizontal) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(horizontal, 0f, 0f), .2f, colisores))
                {
                    movePoint.position += new Vector3(horizontal, 0f, 0f);

                }
            }

            if (Mathf.Abs(vertical) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, vertical, 0f), .2f, colisores))
                {
                    movePoint.position += new Vector3(0f, vertical, 0f);

                }
            }
        }



    }




}
