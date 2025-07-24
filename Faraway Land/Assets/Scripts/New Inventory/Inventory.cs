using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private struct ItemContent
    {
        public int amount;
        public FuckingItemDataBaby data;

        public ItemContent(FuckingItemDataBaby itemData)
        {
            this.amount = 1;
            this.data = itemData;
        }

        public void IncrementAmount(int increment = 1)
        {
            this.amount += increment;
        }
    }

    private List<ItemContent> _content;

    public Inventory()
    {
        this._content = new List<ItemContent>();
    }

    public void AddItem(FuckingItemDataBaby itemData)
    {
        for (int i = 0; i < this._content.Count; i++)
        {
            if (this._content[i].data == itemData)
            {
                this._content[i].IncrementAmount();
                return;
            }
        }
        ItemContent newItemContent = new ItemContent(itemData);
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
}
