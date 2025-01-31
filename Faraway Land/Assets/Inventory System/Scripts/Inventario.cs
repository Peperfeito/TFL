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
    [SerializeField] GameObject DescriPrefab;
    private GameObject dialogoNaTela;
    [SerializeField] GameObject DescriContent;

    public List<Item> itens = new List<Item>();
    public List<Item> itensDesc = new List<Item>();



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
    public bool AddItemDes(Item item)
    {
        if (itensDesc.Contains(item)) return false;

        itensDesc.Add(item);
        UpdateDescriUI();

        return true;



    }

    public void UpdateDescriUI()
    {
        foreach (Transform ditemUI in DescriContent.GetComponentsInChildren<Transform>())
        {
            if (ditemUI.gameObject == DescriContent)
            { continue; }
            Destroy(ditemUI.gameObject);


        }
        Debug.Log("Encontrei nada");

        for (int i = 0; i < itensDesc.Count; i++)
        {
            Debug.Log("Encontrei slaaaa");
            dialogoNaTela = Instantiate(DescriPrefab, DescriContent.transform);
            
            dialogoNaTela.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itensDesc[i].itemName;
        }
    }

    public bool RemoveItem(Item item)
    {
        if (!itens.Contains(item)) return false;

        itens.Remove(item);
        UpdateUI();
        return true;
    }
    public bool VisualizarInventario()
    {
        bool aux = !inventario.activeSelf;
        inventario.SetActive(aux);

        return aux;


    }

    public bool RemoveItemDesc(Item item)
    {
        if(!itensDesc.Contains(item)) return false;

        itensDesc.Remove(item);
        UpdateDescriUI();

        return true;
    }


    public void DestruirInstancia()
    {
        if (dialogoNaTela != null)
        {
            Destroy(dialogoNaTela);
            dialogoNaTela = null; 
        }
    }



    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
        
    }
}
