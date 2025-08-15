using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class FuckingItemDataBaby : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;

    [Tooltip("Visual para o item NO INVENTARIO")]
    public Sprite itemIcon;

    [Tooltip("Visual para o item NO MUNDO")]
    public RuntimeAnimatorController itemAnimatorController;

    public ItemType itemType;

    public CustomEvent itemEvent;
}
