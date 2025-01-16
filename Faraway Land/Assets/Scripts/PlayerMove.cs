using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private bool facingDireita = true;
    private float movedirection;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;
    [SerializeField] private GameObject Sidescroll;
    [SerializeField] private GameObject Grid;
    [SerializeField] private Transform Saida;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
  

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Porta"))
        {
            Sidescroll.SetActive(false);
            Grid.SetActive(true);
            transform.position = Saida.position;
            FindObjectOfType<FadeEffect>().FadeOut();

        }
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
}
