using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    
    [SerializeField] Dialogos dialogo;
    private Inventario inventarioAcess;

    public void AtivarInteracao()
    {
        inventarioAcess.UpdateDescriUI(true, dialogo.neutra);
    }

    private void Awake()
    {
        inventarioAcess = GameObject.Find("Inventario").GetComponent<Inventario>();
    }
}
