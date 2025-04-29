using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGrid : Player
{
    public LayerMask colisores;

    public Transform movePoint;
    private float horizontal;
    private float vertical;
    
    private BoxCollider2D _boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        
        this._boxCollider = this.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        UpdateWaypointPosition();
        MoveTowardsWaypoint();
    }

    private void MoveTowardsWaypoint()
    {
        if ((transform.position - movePoint.position).magnitude <= 0f) return;
        
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    private void UpdateWaypointPosition()
    {
        if (Mathf.Abs(horizontal) >= 1f && Mathf.Abs(vertical) >= 1f) return;

        RaycastHit2D hitInfo = Physics2D.BoxCast((Vector2)(this.transform.position) + this._boxCollider.offset, this._boxCollider.size, 0f, new Vector2(horizontal, vertical), 1, colisores);

        if (!playerPodeSeMover || hitInfo.collider != null) return;

        if ((transform.position - movePoint.position).magnitude <= 0f)
        {
            movePoint.position += new Vector3(horizontal, vertical, 0f);
        }
    }

    

    protected override void OnTriggerEnter2DReaction(Collider2D collision)
    {
        if (collision.CompareTag("Porta"))
        {
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
