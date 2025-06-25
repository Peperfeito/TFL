using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BocaDoPaiacuController : MinigameController
{
    [SerializeField] private Bolinha _bolinha;

    [SerializeField] Transform _ancora;
    private Transform[] _ancorasDeMovimento;
    private int _ancoraIndex;

    [SerializeField] private GameObject _barraDeForca;
    private GameObject _barraDeForcaInstance;
    private Image _medidorDeForca;

    [SerializeField] private GameObject _grade;
    [SerializeField] private GameObject _porta;

    private Animator _clownPuzzleLightAnimator;

    private Vector3 lastPosition;
    private Vector3 escalaOriginal = Vector3.one * 10f;
    private float distanciaMaxima = 20f;

    private bool _ballThrown = false;
    private bool _trackingForce = false;

    private float _forceBarDirection = 1f;
    private float _forceMultiplier = 100f;

    private float _horizontalSpeed = 20f;
    private float _verticalSpeed = 5f;

    protected override void Start()
    {
        this._clownPuzzleLightAnimator = GameObject.Find("Geovanna").GetComponent<Animator>();
        this._ancorasDeMovimento = this._ancora.GetComponentsInChildren<Transform>();

        base.Start();
    }

    private void Update()
    {
        if (!this._isRunning) { return; }

        Vector3 distancia = this._bolinha.transform.position - this._ancorasDeMovimento[0].position;
        float t = Mathf.Clamp01(distancia.y / distanciaMaxima);

        this._bolinha.transform.localScale = Vector3.one * Mathf.Lerp(20f, 1f, t);

        if (Input.GetKeyDown(KeyCode.Escape)) { this.DisableMinigame(); }

        Vector3 posAtual = this._bolinha.transform.position;

        bool xParado = Mathf.Approximately(posAtual.x, lastPosition.x);
        bool yParado = Mathf.Approximately(posAtual.y, lastPosition.y);

        if (xParado && yParado)
        {
            ResetBall();
        }

        if (!this._ballThrown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._trackingForce = true;
            }

            this._medidorDeForca.fillAmount += this._trackingForce ? Time.deltaTime * this._forceBarDirection : 0f;
            
            if (this._medidorDeForca.fillAmount >= 1f || this._medidorDeForca.fillAmount <= 0f)
            {
                this._forceBarDirection *= -1;
            }

            if (!this._trackingForce)
            {
                lastPosition = this._bolinha.transform.position;

                if (Vector2.Distance(this._bolinha.transform.position, this._ancorasDeMovimento[this._ancoraIndex].position) < 0.05f)
                {
                    this._ancoraIndex = (++this._ancoraIndex % this._ancorasDeMovimento.Length);
                }

                this._bolinha.transform.position = Vector2.MoveTowards(this._bolinha.transform.position, this._ancorasDeMovimento[this._ancoraIndex].position, this._horizontalSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._trackingForce = false;
                this._ballThrown = true;
                StartCoroutine(ThrowBall());
            }
        }
    }

    public override void EnableMinigame()
    {
        if (this._isComplete) { return; }

        this._minigameObject.SetActive(true);

        this._playerCamera = Camera.main;
        this._playerCamera.gameObject.SetActive(false);

        this._barraDeForcaInstance = Instantiate(this._barraDeForca, GameObject.Find("GameplayCanvas").transform);
        this._medidorDeForca = this._barraDeForcaInstance.transform.GetChild(0).GetComponent<Image>();

        FindObjectOfType<FadeEffect>().FadeOut();

        this._bolinha.transform.position = this._ancora.position;
        lastPosition = this._bolinha.transform.position;

        this._isRunning = true;
    }

    public override void DisableMinigame()
    {
        this._playerCamera.gameObject.SetActive(true);
        this._minigameObject.SetActive(false);
        Destroy(this._barraDeForcaInstance);

        this._isRunning = false;
    }

    public override void CompleteMinigame()
    {
        this._grade.SetActive(false);
        this._porta.SetActive(true);

        this._clownPuzzleLightAnimator.Play("ClownPuzzleFinishedAnim");

        this._isComplete = true;
        this.DisableMinigame();
    }

    private IEnumerator ThrowBall()
    {
        Vector3 targetPosition = this._bolinha.transform.position + (Vector3.up * (this._medidorDeForca.fillAmount * this._forceMultiplier));

        while (Vector3.Distance(this._bolinha.transform.position, targetPosition) > 0.1f)
        {
            this._bolinha.transform.position = Vector3.MoveTowards(this._bolinha.transform.position, targetPosition, this._verticalSpeed * Time.deltaTime);
            yield return null;
        }
        
        this._bolinha.transform.position = targetPosition;

        yield return new WaitForSeconds(1f);

        if (this._bolinha.transform.position == targetPosition)
        {
            ResetBall();
        }
    }

    public void ResetBall()
    {
        StopCoroutine(nameof(ThrowBall));
        this._bolinha.transform.position = this._ancora.position;
        this._ballThrown = false;
    }
}
