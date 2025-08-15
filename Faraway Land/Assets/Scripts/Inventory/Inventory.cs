using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private class InventorySlot
    {
        public FuckingItemDataBaby data;
        public int amount;

        public InventorySlot(FuckingItemDataBaby itemData)
        {
            this.data = itemData;
            this.amount = 1;
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

    public bool UseItem(FuckingItemDataBaby itemData)
    {
        for (int i = 0; i < this._content.Count; i++)
        {
            if (this._content[i].data == itemData)
            {
                switch (itemData.itemType)
                {
                    case ItemType.None: break;
                    case ItemType.Activatable:
                        itemData.itemEvent.Trigger();
                        break;
                    case ItemType.Consumable: break;
                    case ItemType.Headwear: break;
                    case ItemType.Holdable: break;
                    case ItemType.Footwear: break;
                }

                return true;
            }
        }

        return false;
    }

    public bool LickItem(FuckingItemDataBaby itemData)
    {
        for (int i = 0; i < this._content.Count; i++)
        {
            if (this._content[i].data == itemData)
            {
                // DROPTA O ITEM

                return true;
            }
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
