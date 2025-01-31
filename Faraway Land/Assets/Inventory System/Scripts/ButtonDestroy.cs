using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDestroy : MonoBehaviour
{
    [SerializeField] GameObject prefabs;
    [SerializeField] GameObject buttons;
    protected ItemProp itemProp;
    protected Player player;
    public Inventario inventario;
    public void Deixar()
    {
        buttons.SetActive(false);
        inventario.DestruirInstancia();
        Inventario.Singleton.RemoveItemDesc(itemProp.GetItem());

    }

    public void Pegar()
    {
        buttons.SetActive(false);
        inventario.DestruirInstancia();
        Inventario.Singleton.RemoveItemDesc(itemProp.GetItem());
        player.PegarIten();

    }
}
