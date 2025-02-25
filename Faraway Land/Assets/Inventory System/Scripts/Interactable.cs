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
   


    public void AtivarInteracao()
    {
        
        eventSys.GetComponent<EventSystem>().SetSelectedGameObject(botaoSim);
        botoesInteragir.SetActive(true);
        Inventario.Singleton.UpdateDescriUI(true, dialogo.neutra);
        
        
        
    }

    

    





}
