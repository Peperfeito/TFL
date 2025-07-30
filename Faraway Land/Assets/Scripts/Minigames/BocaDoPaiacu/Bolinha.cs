using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bolinha : MonoBehaviour
{
    private BocaDoPaiacuController _minigame = null;
    public BocaDoPaiacuController Minigame
    {
        get { return this._minigame; }
        
        set
        {
            if (this._minigame == null) this._minigame = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fora"))
        {
            this._minigame.ResetBall();
        }

        if (collision.gameObject.CompareTag("Objetivo"))
        {
            this._minigame.CompleteMinigame();
        }
    }
}
