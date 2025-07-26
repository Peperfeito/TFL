using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class FuckingItemDataBaby : ScriptableObject
{
    public string itemName;
    public string itemDescription;

    public Sprite itemIcon;
    public RuntimeAnimatorController itemAnimatorController;
}
