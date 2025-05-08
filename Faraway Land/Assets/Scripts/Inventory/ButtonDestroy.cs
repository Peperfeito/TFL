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
        GameManager.Instance.Inventory.UpdateDescriUI(false );
    }

    public void Pegar()
    {
        buttons.SetActive(false);
        GameManager.Instance.Inventory.UpdateDescriUI(false);

        GameManager.Instance.Inventory.PegarMarcelo();
    }

    public void SimUsar()
    {
        if (consu == null) return;

        if (GameManager.Instance.Inventory.RemoveItem(consu))
        {
            buttonsPegar.SetActive(false);
            Xbutton.SetActive(true);
            GameManager.Instance.Inventory.UpdateDescriUI(true, dialogo.positiva);
        }
        else 
        {
            buttonsPegar.SetActive(false);
            Xbutton.SetActive(true);
            GameManager.Instance.Inventory.UpdateDescriUI(true, dialogo.negativa);
        }
    }

    public void X()
    {
        buttonsPegar.SetActive(false);
        Xbutton.SetActive(false);
        GameManager.Instance.Inventory.UpdateDescriUI(false);
    }
}
