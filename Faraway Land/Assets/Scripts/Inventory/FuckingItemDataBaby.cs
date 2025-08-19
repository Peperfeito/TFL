using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class FuckingItemDataBaby : ScriptableObject
{
    [Header("MetaData")]
    public string itemName;
    [TextArea] public string itemDescription;
    [Header("Visuals")]
    [Tooltip("Visual para o item NO INVENTARIO")]
    public Sprite itemIcon;
    [Tooltip("Visual estatico para o item NO MUNDO")]
    public Sprite itemWorldSprite;
    [Tooltip("Visual animado para o item NO MUNDO")]
    public RuntimeAnimatorController itemAnimatorController;
    [Header("Sla, mais coisa util")]
    public ItemType itemType;
    public CustomEvent itemEvent;
}
