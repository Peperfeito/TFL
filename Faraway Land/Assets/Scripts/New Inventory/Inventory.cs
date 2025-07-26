using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InventoryContent
{
    public int amount;
    public FuckingItemDataBaby data;

    public InventoryContent(FuckingItemDataBaby itemData, int currentAmount)
    {
        this.amount = currentAmount;
        this.data = itemData;
    }
}

public class Inventory
{
    private class InventorySlot
    {
        public int amount;
        public FuckingItemDataBaby data;

        public InventorySlot(FuckingItemDataBaby itemData)
        {
            this.amount = 1;
            this.data = itemData;
        }

        public void IncrementAmount(int increment)
        {
            this.amount += increment;
        }
    }

    private List<InventorySlot> _content;

    public Inventory()
    {
        this._content = new List<InventorySlot>();
    }

    public void AddItem(FuckingItemDataBaby itemData)
    {
        for (int i = 0; i < this._content.Count; i++)
        {
            if (this._content[i].data == itemData)
            {
                this._content[i].IncrementAmount(1);
                return;
            }
        }
        InventorySlot newItemContent = new InventorySlot(itemData);
        this._content.Add(newItemContent);
    }

    public void RemoveItem(FuckingItemDataBaby itemData)
    {
        for (int i = 0; i < this._content.Count; i++)
        {
            if (this._content[i].data == itemData)
            {
                this._content[i].IncrementAmount(-1);

                if (this._content[i].amount <= 0)
                {
                    this._content.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public bool HasItem(FuckingItemDataBaby itemData)
    {
        for (int i = 0; i < this._content.Count; i++)
        {
            if (this._content[i].data == itemData) { return true; }
        }
        return false;
    }

    public InventoryContent[] GetContents()
    {
        InventoryContent[] result = new InventoryContent[this._content.Count];

        for (int i = 0; i < this._content.Count; i++)
        {
            result[i] = new InventoryContent(this._content[i].data, this._content[i].amount);
        }

        return result;
    }
}
