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
    private Inventario inventarioAcess;



    public void Deixar()
    {
        buttons.SetActive(false);
        inventarioAcess.UpdateDescriUI(false );
        

    }

    public void Pegar()
    {
        buttons.SetActive(false);
        inventarioAcess.UpdateDescriUI(false);

        inventarioAcess.PegarMarcelo();

    }

    public void SimUsar()
    {
        if (consu == null) return;

        if (inventarioAcess.RemoveItem(consu))
        {
            buttonsPegar.SetActive(false);
            Xbutton.SetActive(true);
            inventarioAcess.UpdateDescriUI(true, dialogo.positiva);
        }
        else 
        {
            buttonsPegar.SetActive(false);
            Xbutton.SetActive(true);
            inventarioAcess.UpdateDescriUI(true, dialogo.negativa);

        }
        

    }

    

    public void X()
    {
        buttonsPegar.SetActive(false);
        Xbutton.SetActive(false);
        inventarioAcess.UpdateDescriUI(false);
    }

    private void Awake()
    {
        inventarioAcess = GameObject.Find("Inventario").GetComponent<Inventario>();
    }


}
