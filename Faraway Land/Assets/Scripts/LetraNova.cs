using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static LetraNova;

public class LetraNova : MonoBehaviour
{
    public bool stopDrag = false;
    public Color objectColor;
    private Vector3 _mousePositionOffset;

    public enum Letra { M, E, I, Errada }
    public Letra minhaLetra;
    public GameObject objetoCerto;
    private SpriteRenderer _lateral;
    private Rigidbody2D _rigidbody2D;
    private BlocoDiLetraController _telemacuuuuuuuuu;

    private void Start()
    {
        this._rigidbody2D = GetComponent<Rigidbody2D>();
        this._lateral = GetComponent<SpriteRenderer>();
        this.ResetPosition();
        this._telemacuuuuuuuuu = GameObject.FindAnyObjectByType<BlocoDiLetraController>();
    }

    private Vector3 GetMouseWordPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseEnter()
    {
        stopDrag = false;
    }

    private void OnMouseUp()
    {
        _lateral.color = Color.white;
        _rigidbody2D.gravityScale = 1;
    }

    private void OnMouseDrag()
    {
        if (!stopDrag)
        {
            transform.position = GetMouseWordPosition() + _mousePositionOffset;
            _rigidbody2D.gravityScale = 0;
        }
    }

    private void OnMouseDown()
    {
        _lateral.color = objectColor;
        _mousePositionOffset = gameObject.transform.position - GetMouseWordPosition();
    }

    public void ResetPosition()
    {
        transform.position = transform.parent.position;
        _lateral.color = Color.white;
    }

    private void StopDrag()
    {
        stopDrag = true;
        ResetPosition();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (minhaLetra)
        {
            case Letra.M:
                if (collision.gameObject.CompareTag("espaco"))
                {
                    collision.gameObject.SetActive(false);
                    objetoCerto.SetActive(true);
                    VerificarLetra();
                }
                StopDrag();
                break;

            case Letra.E:
                if (collision.gameObject.CompareTag("espaco1"))
                {
                    collision.gameObject.SetActive(false);
                    objetoCerto.SetActive(true);
                    VerificarLetra();
                }
                StopDrag();
                break;

            case Letra.I:
                if (collision.gameObject.CompareTag("espaco2"))
                {
                    collision.gameObject.SetActive(false);
                    objetoCerto.SetActive(true);
                    VerificarLetra();
                }
                StopDrag();
                break;

            case Letra.Errada:
                StopDrag();
                break;
        }
    }

    public void VerificarLetra()
    {
        if (this._telemacuuuuuuuuu == null) this._telemacuuuuuuuuu = GameObject.FindAnyObjectByType<BlocoDiLetraController>();
        this._telemacuuuuuuuuu.Verificar();
    }
}
