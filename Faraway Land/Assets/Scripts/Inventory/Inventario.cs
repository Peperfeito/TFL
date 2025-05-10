using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    [SerializeField] public GameObject inventario;
    [SerializeField] GameObject inventarioContent;
    [SerializeField] GameObject itemPrefab;

    [SerializeField] private LevelUIController _levelUIController; // serializado por preguica, dps mudar pra encontrar a instancia atual

    public List<Item> itens = new List<Item>();
    public ItemProp marcelo; // O marcelo eh um buffer

    public bool AddItem(Item item)
    {
        if (itens.Contains(item)) return false;

        itens.Add(item);
        UpdateUI();

        return true;
    }

    public void UpdateUI()
    {
        foreach (Transform itemUI in inventarioContent.GetComponentsInChildren<Transform>())
        {
            if (itemUI.gameObject == inventarioContent) { continue; }
            Destroy(itemUI.gameObject);
        }

        for (int i = 0; i < itens.Count; i++)
        {
            GameObject itemNaTela = Instantiate(itemPrefab, inventarioContent.transform);
            itemNaTela.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itens[i].itemName;
            itemNaTela.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itens[i].itemDescri;
        }
    }


    public void UpdateItemBuff(ItemProp itemProp)
    {
        marcelo = itemProp;
        Item currentItem = marcelo.GetItem();
        this._spriteBuffer = currentItem.itemIcone;
        this._levelUIController.UpdateDialogBox(DialogBoxMode.ItemInteraction, currentItem.itemIcone, $"Voce encontrou <color=#ffff00>{currentItem.itemName}</color>");
    }

    private Sprite _spriteBuffer;

    public void UpdateDescriUI(bool ativo, string textoDia = "")
    {
        this._levelUIController.UpdateDialogBox(DialogBoxMode.ObjectInteraction, this._spriteBuffer, textoDia); // PLACEHOLDER MT PORCO
        //dialogoNaTela.SetActive(ativo);

        //if (dialogoNaTela.activeSelf)
        //{
        //    dialogoNaTela.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = textoDia;
        //}
    }

    public void PegarMarcelo()
    {
        if (marcelo == null) return;

        AddItem(marcelo.GetItem());
        marcelo.gameObject.SetActive(false);
        this._spriteBuffer = marcelo.GetItem().itemIcone;
        marcelo = null;
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

    public bool DialogoNaTela() => this._levelUIController.DialogBox.activeSelf;
}
