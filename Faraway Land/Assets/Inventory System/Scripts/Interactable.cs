using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    [SerializeField] Item consu;
    [SerializeField] Dialogos dialogo;
    

    public void AtivarInteracao()
    {
        
        Debug.Log(dialogo.neutra);
        if(consu == null) return;

        if(Inventario.Singleton.RemoveItem(consu))
        {
            Debug.Log(dialogo.positiva);
        }
        else 
        { 
            Debug.Log(dialogo.negativa); 
        }
        
    }



}
