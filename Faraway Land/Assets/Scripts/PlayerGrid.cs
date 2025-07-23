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

    [SerializeField] private GameObject _staminaIndicator;
    [SerializeField] private Transform _staminaBar;
    private float _stamina = 1f;
    private bool _isSprinting = false;
    private float _sprintMult = 1.7f;
    private float _staminaRechargeDelay = 0f;
    private float _indicatorFadeDelay = 0f;

    //[SerializeField] private AudioClip[] passos;
    //[SerializeField] private AudioSource audios;

    private void Start()
    {
        Vector3 positionBuffer = transform.position; //Camilla eh um buffer
        positionBuffer.x = (Mathf.Floor(Mathf.Abs(positionBuffer.x)) + .5f) * (positionBuffer.x / Mathf.Abs(positionBuffer.x));
        positionBuffer.y = Mathf.Round(positionBuffer.y) + .3f;
        transform.position = positionBuffer;

        movePoint.parent = null;

        this._boxCollider = this.GetComponent<BoxCollider2D>();

    }

    private void StartSprint()
    {
        this._isSprinting = true; this._staminaRechargeDelay = 0f; this._indicatorFadeDelay = 2f;
    }

    private void StopSprint()
    {
        this._isSprinting = false; this._staminaRechargeDelay = 2f;
    }

    private void Update()
    {
        InputHandler();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        /* Sprint e Stamina */
        if (Input.GetKeyDown(KeyCode.LeftShift)) this.StartSprint();
        if (Input.GetKeyUp(KeyCode.LeftShift)) this.StopSprint();

        // TODO: resetar o rechard quando this._isSprinting mas soltar o movimento
        this._staminaRechargeDelay -= Time.deltaTime;
        if (this._staminaRechargeDelay <= 0f)
        {
            this._staminaRechargeDelay = 0f;
            if (this._isSprinting && animationState == "Walk")
            {
                this._stamina -= Time.deltaTime / 5f;
                if (this._stamina <= 0f) this._stamina = 0f;
            }
            else
            {
                this._stamina += Time.deltaTime / 10f;
                if (this._stamina >= 1f) this._stamina = 1f;
            }
        }

        if (this._stamina >= 1f)
        {
            this._indicatorFadeDelay -= Time.deltaTime;
            if (this._indicatorFadeDelay <= 0f) this._indicatorFadeDelay = 0f;
        }

        if (this._isSprinting && this._stamina <= 0f) { this.StopSprint(); } // acabou enquanto corria

        this._staminaIndicator.SetActive(this._stamina < 1f || this._indicatorFadeDelay > 0f);

        this._staminaBar.localScale = new Vector3(1f, this._stamina, 1f);
        /* -- */

        UpdateWaypointPosition();
        MoveTowardsWaypoint();

        this._animator.Play($"{animationState}{animationDirection}");
    }

    private float animationChangeThreshold = .1f;
    private string animationState = "Idle";
    private string animationDirection = "Down";

    private void MoveTowardsWaypoint()
    {


        animationState = "Idle";
        //audios.Pause();
        animationDirection = (horizontal >= animationChangeThreshold ? "Right" : (horizontal <= -animationChangeThreshold ? "Left" : (vertical >= animationChangeThreshold ? "Up" : (vertical <= -animationChangeThreshold ? "Down" : animationDirection))));

        if ((transform.position - movePoint.position).magnitude <= 0f) return;

        animationState = "Walk";
        //audios.Play();


        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime * (this._isSprinting && this._stamina > 0f ? this._sprintMult : 1f));

        Vector3 direction = movePoint.position - transform.position;

        animationDirection = (direction.x >= animationChangeThreshold ? "Right" : (direction.x <= -animationChangeThreshold ? "Left" : (direction.y >= animationChangeThreshold ? "Up" : (direction.y <= -animationChangeThreshold ? "Down" : animationDirection))));
    }

    private void UpdateWaypointPosition()
    {
        if (Mathf.Abs(horizontal) >= 1f && Mathf.Abs(vertical) >= 1f) return;

        RaycastHit2D hitInfo = Physics2D.BoxCast((Vector2)(this.transform.position) + this._boxCollider.offset, this._boxCollider.size, 0f, new Vector2(horizontal, vertical), 1, colisores);

        if (hitInfo.collider != null && hitInfo.collider.CompareTag("Empurravel"))
        {
            hitInfo.collider.transform.GetComponent<Stool>().Pusher(transform.position);
        }

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

        if (collision.CompareTag("Amoeba"))
        {
            //audios.clip = passos[1];


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
            //audios.clip = passos[0];
        }



        OnTriggerExit2DReaction(collision);
    }


}
