using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDestroy : MonoBehaviour
{
    [SerializeField] GameObject prefabs;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject buttonsPegar;
    [SerializeField] GameObject Xbutton;
    [SerializeField] Dialogos dialogo;
    [SerializeField] Item consu;
    protected ItemProp itemProp;
    public Interactable interactable;
    


    public void Deixar()
    {
        buttons.SetActive(false);
        Inventario.Singleton.UpdateDescriUI(false );
        

    }

    public void Pegar()
    {
        buttons.SetActive(false);
        Inventario.Singleton.UpdateDescriUI(false);

        Inventario.Singleton.PegarMarcelo();

    }

    public void SimUsar()
    {
        if (consu == null) return;

        if (Inventario.Singleton.RemoveItem(consu))
        {
            interactable.eventSys.GetComponent<EventSystem>().SetSelectedGameObject(Xbutton);
            buttonsPegar.SetActive(false);
            Xbutton.SetActive(true);
            Inventario.Singleton.UpdateDescriUI(true, dialogo.positiva);
        }
        else 
        {
            interactable.eventSys.GetComponent<EventSystem>().SetSelectedGameObject(Xbutton);
            buttonsPegar.SetActive(false);
            Xbutton.SetActive(true);
            Inventario.Singleton.UpdateDescriUI(true, dialogo.negativa);

        }
        

    }

    

    public void X()
    {
        buttonsPegar.SetActive(false);
        Xbutton.SetActive(false);
        Inventario.Singleton.UpdateDescriUI(false);
    }

    
}
