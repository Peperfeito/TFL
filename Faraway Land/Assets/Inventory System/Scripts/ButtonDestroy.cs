using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDestroy : MonoBehaviour
{
    [SerializeField] GameObject prefabs;
    [SerializeField] GameObject buttons;
    protected ItemProp itemProp;
    
    
    public void Deixar()
    {
        buttons.SetActive(false);
        Inventario.Singleton.UpdateDescriUI(false);
        

    }

    public void Pegar()
    {
        buttons.SetActive(false);
        Inventario.Singleton.UpdateDescriUI(false);

        Inventario.Singleton.PegarMarcelo();

    }
}
