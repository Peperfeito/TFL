using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    [SerializeField] public GameObject inventario;
    [SerializeField] GameObject inventarioContent;
    [SerializeField] GameObject itemPrefab;
    
    public GameObject dialogoNaTela;
    [SerializeField] GameObject DescriContent;


    public List<Item> itens = new List<Item>();
    public ItemProp marcelo; //O marcelo eh um buffer



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
    

    public void UpdateItemBuff(ItemProp itemProp)
    {
        marcelo = itemProp;
        UpdateDescriUI(true, marcelo.GetItem().itemName);
    }

    public void UpdateDescriUI(bool ativo, string textoDia = "")
    {
        dialogoNaTela.SetActive(ativo);
        if(dialogoNaTela.activeSelf)
        {
            dialogoNaTela.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = textoDia;
        }


    }

    public void PegarMarcelo()
    {
        if (marcelo != null)
        {





            AddItem(marcelo.GetItem());

            marcelo.gameObject.SetActive(false);


            marcelo = null;


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

    


    



    private void Awake()
    {
      
        DontDestroyOnLoad(this.gameObject);
        
    }
}
