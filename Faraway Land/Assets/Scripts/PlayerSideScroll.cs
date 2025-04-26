using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideScroll : Player
{
    
    private Rigidbody2D rb;
    private bool facingDireita = true;
    private float movedirection;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;
    public Player player;
    
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
  

    // Update is called once per frame
    void Update()
    {
        if (!playerPodeSeMover)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        player.InputHandler();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if(!playerPodeSeMover) return;
         if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if(movedirection > 0 && !facingDireita)
        {
            Flip();
        }
        else if(movedirection < 0 && facingDireita)
        {
            Flip();
        }

        movedirection = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(movedirection * moveSpeed, rb.velocity.y);
        

      
    }

    protected override void OnTriggerEnter2DReaction(Collider2D collision)
    {
        if (collision.CompareTag("Porta"))
        {
            Sidescroll.SetActive(false);
            Grid.SetActive(true);
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

    

    void Jump()
    {
        
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void Flip()
    {
        facingDireita = !facingDireita;
        transform.Rotate(0f, 180f,0f);
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
