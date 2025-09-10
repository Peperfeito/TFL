using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameCaller : MonoBehaviour
{
    [SerializeField] private Minigames _minigame;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.ChamaMinigame(this._minigame);
        }
    }
}
