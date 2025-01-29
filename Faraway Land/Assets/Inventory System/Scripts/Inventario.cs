using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    [SerializeField] GameObject inventario;
    [SerializeField] GameObject inventarioContent;
    [SerializeField] GameObject itemPrefab;
    public static Inventario Singleton;
    List<Item> itens = new List<Item>();

    public bool AddItem(Item item)
    {
        if(itens.Contains(item)) return false;

        itens.Add(item);
        UpdateUI();

        return true;


        
    }

    public void UpdateUI()
    {
        foreach (Transform itemUI in inventarioContent.GetComponentsInChildren<Transform>()) 
        {
            if (itemUI.gameObject == inventarioContent) 
            { continue; }
            Destroy(itemUI.gameObject);
        
        
        }

        for (int i=0; i < itens.Count; i++)
        {
            GameObject itemNaTela = Instantiate(itemPrefab, inventarioContent.transform);
            itemNaTela.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itens[i].itemName;
            itemNaTela.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itens[i].itemDescri;
        }
        
    }

    public bool VisualizarInventario()
    {
        bool aux = !inventario.activeSelf;
        inventario.SetActive(aux);

        return aux;


    }

    public bool RemoveItem(Item item)
    {
        if(!itens.Contains(item)) return false;

        itens.Remove(item);
        UpdateUI();
        return true;
    }

    



    
    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
        
    }
}
