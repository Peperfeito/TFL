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
    private bool melado = false;
    
    private BoxCollider2D _boxCollider;

    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource passosAudio;
    [SerializeField] private AudioSource passosAudioMelado;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 positionBuffer = transform.position;
        positionBuffer.x = Mathf.Round(positionBuffer.x) + .5f;
        positionBuffer.y = Mathf.Round(positionBuffer.y) + .3f;
        transform.position = positionBuffer;
        
        movePoint.parent = null;
        
        this._boxCollider = this.GetComponent<BoxCollider2D>();
        passosAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        UpdateWaypointPosition();
        MoveTowardsWaypoint();
        if (melado == false)
        {
            if (horizontal < 0 || horizontal > 0 || vertical < 0 || vertical > 0)
            {
                passosAudio.UnPause();
            }
            else
            {
                passosAudio.Pause();
            }
        }

        this._animator.Play($"{animationState}{animationDirection}");
    }

    private float animationChangeThreshold = .1f;
    private string animationState = "Idle";
    private string animationDirection = "Down";

    private void MoveTowardsWaypoint()
    {
        animationState = "Idle";
        animationDirection = (horizontal >= animationChangeThreshold ? "Right" : (horizontal <= -animationChangeThreshold ? "Left" : (vertical >= animationChangeThreshold ? "Up" : (vertical <= -animationChangeThreshold ? "Down" : animationDirection))));
        
        if ((transform.position - movePoint.position).magnitude <= 0f) return;

        animationState = "Walk";
        

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        Vector3 direction = movePoint.position - transform.position;

        animationDirection = (direction.x >= animationChangeThreshold ? "Right" : (direction.x <= -animationChangeThreshold ? "Left" : (direction.y >= animationChangeThreshold ? "Up" : (direction.y <= -animationChangeThreshold ? "Down" : animationDirection))));
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

        if(collision.CompareTag("Amoeba"))
        {
            melado = true;

            if (horizontal < 0 || horizontal > 0 || vertical < 0 || vertical > 0)
            {
                passosAudioMelado.UnPause();
            }
            else
            {
                passosAudioMelado.Pause();
            }
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
        if (collision.CompareTag("Amoeba"))
        {
            melado = false;
        }



            OnTriggerExit2DReaction(collision);
    }

    
}
