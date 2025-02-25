using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsMove : MonoBehaviour
{
    public float speed = 0.1f; // Velocidade do deslocamento
    private Material mat;
    private Vector2 offset;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        offset.x += speed * Time.deltaTime; // Move a textura no eixo X
        mat.mainTextureOffset = offset; // Aplica o deslocamento na textura
    }
    
}
