using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanterna : MonoBehaviour
{
   public GameObject objetoAlvo; // arraste o objeto que você quer ativar/desativar aqui

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (objetoAlvo != null)
            {
                objetoAlvo.SetActive(!objetoAlvo.activeSelf);
            }
        }
    }
}
