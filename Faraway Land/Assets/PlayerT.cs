using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerT : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade do personagem

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Pegando o Rigidbody2D do personagem
    }

    void Update()
    {
        // Pegando a entrada do jogador (WASD ou setas)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; // Normaliza para evitar velocidade maior na diagonal
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed; // Aplica a velocidade ao Rigidbody
    }
}
