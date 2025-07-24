using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private SpriteRenderer _staminaBar;
    [SerializeField] private Color _fullStamina;
    [SerializeField] private Color _halfStamina;
    [SerializeField] private Color _lowStamina;
    [SerializeField] private SpriteRenderer _staminaFrame;
    [SerializeField] private Color _rechargeColor;
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

    private void Update()
    {
        InputHandler();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift)) this._isSprinting = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) this._isSprinting = false;
        this.UpdateStamina();

        UpdateWaypointPosition();
        MoveTowardsWaypoint();

        this._animator.Play($"{this._currentAnimState}{this._animationDirection}");
    }

    private float _animationChangeThreshold = .1f;
    private string _currentAnimState = "Idle";
    private string _animationDirection = "Down";

    private void UpdateStamina()
    {
        // Delay before stamina starts recharging right after sprint has ended (either by stop srpinting or depleting all stamina)
        this._staminaRechargeDelay -= Time.deltaTime;
        if (this._staminaRechargeDelay <= 0f) { this._staminaRechargeDelay = 0f; } // clamp

        // Use stamina to sprint
        if (this._currentAnimState == "Walk" && this._isSprinting)
        {
            this._stamina -= Time.deltaTime / 7f; // Takes 7 seconds to fully depleat
            if (this._stamina <= 0f) this._stamina = 0f; // clamp
            
            this._indicatorFadeDelay = 2f;
            this._staminaRechargeDelay = 2f;

            this._staminaFrame.color = Color.white; // visual
        }
        
        // Rechard stamina
        if (this._staminaRechargeDelay <= 0f && (!this._isSprinting || this._currentAnimState == "Idle"))
        {
            this._stamina += Time.deltaTime / 10f; // Takes 10 seconds to fully recharge
            if (this._stamina >= 1f) this._stamina = 1f; // clamp

            this._staminaFrame.color = this._rechargeColor; // visual
        }

        // Holds stamina bar in sight while full for a while before disapearing
        if (this._stamina >= 1f)
        {
            this._indicatorFadeDelay -= Time.deltaTime;
            if (this._indicatorFadeDelay <= 0f) this._indicatorFadeDelay = 0f;

            this._staminaFrame.color = Color.white; // visual
        }

        // Ends sprint by stamina depletion
        if (this._isSprinting && this._stamina <= 0f) { this._isSprinting = false; }

        // Stamina bar visual updates
        this._staminaIndicator.SetActive(this._stamina < 1f || this._indicatorFadeDelay > 0f);
        this._staminaBar.transform.localScale = new Vector3(1f, this._stamina, 1f);
        this._staminaBar.color =
            this._stamina > .5f ?
            this.ColorLerp(this._fullStamina, this._halfStamina, (1 - this._stamina) * 2) :
            this.ColorLerp(this._lowStamina, this._halfStamina, this._stamina * 2) ;
    }

    private Color ColorLerp(Color a, Color b, float t) // move to Util later, cuz I lazy
    {
        float newR = Mathf.Lerp(a.r, b.r, t);
        float newG = Mathf.Lerp(a.g, b.g, t);
        float newB = Mathf.Lerp(a.b, b.b, t);
        float newA = Mathf.Lerp(a.a, b.a, t);

        return new Color(newR, newG, newB, newA);
    }

    private void MoveTowardsWaypoint()
    {
        this._currentAnimState = "Idle";
        //audios.Pause();
        this._animationDirection = (horizontal >= this._animationChangeThreshold ? "Right" : (horizontal <= -this._animationChangeThreshold ? "Left" : (vertical >= this._animationChangeThreshold ? "Up" : (vertical <= -this._animationChangeThreshold ? "Down" : this._animationDirection))));
        
        if ((transform.position - movePoint.position).magnitude <= 0f) { return; }

        this._currentAnimState = "Walk";
        //audios.Play();


        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime * (this._isSprinting && this._stamina > 0f ? this._sprintMult : 1f));

        Vector3 direction = movePoint.position - transform.position;

        this._animationDirection = (direction.x >= this._animationChangeThreshold ? "Right" : (direction.x <= -this._animationChangeThreshold ? "Left" : (direction.y >= this._animationChangeThreshold ? "Up" : (direction.y <= -this._animationChangeThreshold ? "Down" : this._animationDirection))));
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
