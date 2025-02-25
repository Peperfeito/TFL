using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : Player
{
    public LayerMask colisores;

    
    public Transform movePoint;
    
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();

        if (!playerPodeSeMover) return;

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {

        if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {
            if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, colisores))
            {
            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            
            }
        }

                if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, colisores))
            {
            movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            
            }
        }
        }
    }

    protected override void InputHandler()
    {
        base.InputHandler();

    }

    protected override void OnTriggerEnter2DReaction(Collider2D collision)
    {
        if (collision.CompareTag("Porta"))
        {
            Sidescroll.SetActive(true);
            Grid.SetActive(false);
            movePoint.position = Saida.position;
            transform.position = Saida.position;
            FindObjectOfType<FadeEffect>().FadeOut();
            
            return;
        }

        base.OnTriggerEnter2DReaction(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        OnTriggerEnter2DReaction(collision);
    }

    protected override void OnTriggerExit2DReaction(Collider2D collision)
    {
        base.OnTriggerExit2DReaction(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit2DReaction(collision);
    }
}
