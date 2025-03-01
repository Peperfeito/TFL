using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    
    [SerializeField] Dialogos dialogo;
    [SerializeField] public GameObject eventSys;
    [SerializeField] GameObject botoesInteragir;
    [SerializeField] GameObject botaoSim;
    private Inventario inventarioAcess;



    public void AtivarInteracao()
    {
        
        eventSys.GetComponent<EventSystem>().SetSelectedGameObject(botaoSim);
        botoesInteragir.SetActive(true);
        inventarioAcess.UpdateDescriUI(true, dialogo.neutra);
        
        
        
    }

    private void Awake()
    {
        inventarioAcess = GameObject.Find("Inventario").GetComponent<Inventario>();
    }







}
